using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using pool;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Cysharp.Threading.Tasks;
using Shun_Player;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

namespace wave
{
    public class WaveController : MonoBehaviour
    {
        [SerializeField,Header("クリアに設定したいWave数")] private int waveClearCount;
        private int waveAdvanceCount = 0; // ウェーブが進行した数
        [SerializeField] private CanvasShow canvasShow;
        public Subject<int> OnEnemyDestroyed { get; private set; } = new Subject<int>();
        private int destroyedEnemyCount = 0;
        [System.Serializable]
        public class EnemyType
        {
            [SerializeField, Header("敵の種類を設定")] public GameObject enemyPrefab; // 敵のプレハブ
            [SerializeField, Header("出現確率")] public float spawnChance; // 出現確率
            [SerializeField, Header("出現上限")] public int maxSpawnCount; // 最大出現数
            [SerializeField, Header("現在の出現数")] [HideInInspector] public int currentSpawnCount; // 現在の出現数
        }
        [System.Serializable]
        public class Wave
        {
            [SerializeField, Header("このWaveでの敵の合計数")] public int totalEnemies; // Waveの敵の合計数
            [SerializeField, Header("Waveで出現可能な敵の種類の数")] public EnemyType[] enemies; // Waveで出現可能な敵の種類の配列
        }
        [SerializeField, Header("Wave数")]
        public Wave[] waves; // Waveの配列
        [HideInInspector] public int currentWaveIndex = 0; // 現在のWaveのインデックス
        [SerializeField] private int poolSize = 5; // プールサイズ
        private Dictionary<GameObject, EnemyObjPool> enemyPools = new Dictionary<GameObject, EnemyObjPool>(); //敵のプレハブごとのオブジェクトプール
        private CompositeDisposable enemySubscriptions = new CompositeDisposable();
        [SerializeField] private PlayerBase playerObject;
        private bool isPlayerSpawn = false;
        private ReactiveProperty<int> totalActiveEnemies = new ReactiveProperty<int>(0);
        private bool allEnemiesSpawned = false; // ウェーブ内の全ての敵がスポーンしたかどうかを示すフラグ
        public Camera mainCamera; // メインカメラ
        [SerializeField] private int maxActiveEnemies;
        [SerializeField,Header("ステージの左下からのx,y")]private Vector2 stageMinBounds; // ステージの左下
        [SerializeField,Header("ステージの右上からのx,y")]private Vector2 stageMaxBounds; // ステージの右上
        [SerializeField,Header("敵のスポーン間隔、1000で1秒")] private int spawnInterval;

        /// <summary>
        /// ゲーム開始時に呼ばれ、ウェーブと敵の初期化
        /// </summary>
        public async UniTask Init(PlayerBase playerBase)
        {
            Debug.Log(playerBase);
            playerObject = playerBase;
            isPlayerSpawn = true;
            OnPlayerSpawned();
            enemyPools = new Dictionary<GameObject, EnemyObjPool>();
            foreach (Wave wave in waves)
            {
                foreach (EnemyType enemyType in wave.enemies)
                {
                    if (!enemyPools.ContainsKey(enemyType.enemyPrefab))
                    {
                        EnemyObjPool pool = gameObject.AddComponent<EnemyObjPool>();
                        pool.Initialize(enemyType.enemyPrefab, poolSize);
                        enemyPools.Add(enemyType.enemyPrefab, pool);
                    }
                }
            }
            // このキャンセルトークンはこの MonoBehaviourが破棄されたときにキャンセルされる
            var cancellationToken = this.GetCancellationTokenOnDestroy();
            try
            {
                await SpawnWave().AttachExternalCancellation(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // キャンセル時の例外をキャッチし、処理を中断または適切なクリーンアップを行います
                Debug.Log("Init was cancelled due to scene unload.");
            }
        }
        public void OnPlayerSpawned()
        {
            if(isPlayerSpawn){
                mainCamera = Camera.main; // MainCameraのタグ
            }
        }
        /// <summary>
        /// 指定されたウェーブの敵をスポーン
        /// </summary>
        /// <param name="token">非同期タスクのキャンセルトークン</param>
        /// <returns>ウェーブのスポーンが完了したら完了するタスク</returns>
        private async UniTask SpawnWave()
        {
            if(currentWaveIndex < waves.Length)
            {
                // コンポーネントが破壊されるとキャンセルされるトークン
                var ct = this.GetCancellationTokenOnDestroy();

                await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: ct);

                Wave currentWave = waves[currentWaveIndex];
                for (int i = 0; i < currentWave.totalEnemies;)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }

                    if (totalActiveEnemies.Value >= maxActiveEnemies)
                    {
                        await UniTask.WaitUntil(() => totalActiveEnemies.Value < maxActiveEnemies, cancellationToken: ct);
                    }

                    int enemyIndex = GetRandomEnemyIndex(currentWave.enemies);
                    EnemyType chosenEnemy = currentWave.enemies[enemyIndex];
                    if (chosenEnemy.currentSpawnCount < chosenEnemy.maxSpawnCount)
                    {
                        SpawnEnemy(chosenEnemy);
                        chosenEnemy.currentSpawnCount++;
                        i++;
                    }

                    await UniTask.Delay(spawnInterval, cancellationToken: ct); // エネミーのスポーン間隔, 500で0.5秒
                }

                allEnemiesSpawned = true;
                OnAllEnemiesSpawned();

                // 全ての敵が破壊されるのを待つ
                await UniTask.WaitUntil(() => totalActiveEnemies.Value == 0, cancellationToken: ct);

                currentWaveIndex++;
            }
        }
        // すべての敵がスポーンされた後に呼び出されるメソッド
        private void OnAllEnemiesSpawned()
        {
            // 敵の数が0になるのを監視する
            totalActiveEnemies
                .Where(activeEnemies => activeEnemies == 0 && allEnemiesSpawned)
                .Take(1) // 一回だけ処理する
                .Subscribe(_ => ProceedToNextWave())
                .AddTo(this);
        }
        // 次のウェーブに進む処理
        private async UniTask ProceedToNextWave()
        {
            var ct = this.GetCancellationTokenOnDestroy();
            try
            {
                Debug.Log("NextWave");
                waveAdvanceCount++;
                if(waveAdvanceCount == waveClearCount)
                {
                    canvasShow.ClearCanvasShow();
                    await UniTask.Delay(5000, cancellationToken: ct);
                    SceneManager.LoadScene("ClearScene");
                }
                else
                {
                    destroyedEnemyCount = 0;
                    OnEnemyDestroyed.OnNext(destroyedEnemyCount);
                    allEnemiesSpawned = false; // フラグをリセット
                    await SpawnWave().AttachExternalCancellation(ct);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("cancel");
            }
        }
        /// <summary>
        /// 敵のタイプのインデックスをランダムに選択。
        /// 敵の選択は敵のスポーン確率に基づく。
        /// </summary>
        /// <param name="enemies">敵のタイプの配列</param>
        /// <returns>選択された敵のタイプのインデックス</returns>
        private int GetRandomEnemyIndex(EnemyType[] enemies)
        {
            float total = 0;
            float randomPoint = UnityEngine.Random.value;

            for (int i = 0; i < enemies.Length; i++)
            {
                total += enemies[i].spawnChance;
                if (randomPoint <= total)
                {
                    return i;
                }
            }
            throw new System.Exception("a");
        }
        /// <summary>
        /// 指定されたタイプの敵をスポーン
        /// </summary>
        /// <param name="enemyType">スポーンさせる敵のタイプ</param>
        private void SpawnEnemy(EnemyType enemyType)
        {
            GameObject spawnedEnemyObject = enemyPools[enemyType.enemyPrefab].GetObject(); // プールから敵を取得
            if(spawnedEnemyObject == null) return;
            totalActiveEnemies.Value++;
            IEnemy spawnedEnemy = spawnedEnemyObject.GetComponent<IEnemy>();

            // ステージ内でランダムなスポーン位置を生成
            Vector2 spawnPosition = new Vector2(
                UnityEngine.Random.Range(stageMinBounds.x, stageMaxBounds.x),
                UnityEngine.Random.Range(stageMinBounds.y, stageMaxBounds.y)
            );

            spawnedEnemyObject.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);

            //敵が破壊されたときにプールに戻るように設定
            spawnedEnemy.OnDestroyed.Subscribe(async _ =>
            {
                DestroyEnemy(spawnedEnemyObject, spawnedEnemy);
            }).AddTo(this);
        }
            /// <summary>
        /// 指定された敵オブジェクトを破壊
        /// </summary>
        /// <param name="enemyObject">破壊する敵オブジェクト</param>
        /// <param name="enemy">破壊する敵</param>
        private async void DestroyEnemy(GameObject enemyObject, IEnemy enemy)
        {
            totalActiveEnemies.Value--;
            destroyedEnemyCount++;
            OnEnemyDestroyed.OnNext(destroyedEnemyCount);
            enemyObject.SetActive(false);
            enemy.ResetSubscription();
        }
    }
}