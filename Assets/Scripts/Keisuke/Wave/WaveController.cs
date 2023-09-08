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


namespace wave
{
    public class WaveController : MonoBehaviour
    {
        [SerializeField,Header("クリアに設定したいWave数")] private int waveClearCount;
        private int waveAdvanceCount = 0; // ウェーブが進行した数
        [SerializeField] private CanvasShow canvasShow;
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
        private int currentWaveIndex = 0; // 現在のWaveのインデックス
        [SerializeField] private int poolSize = 5; // プールサイズ
        private Dictionary<GameObject, EnemyObjPool> enemyPools = new Dictionary<GameObject, EnemyObjPool>(); //敵のプレハブごとのオブジェクトプール
        private CancellationTokenSource cancelToken;
        private CompositeDisposable enemySubscriptions = new CompositeDisposable();
        [SerializeField] private PlayerBase playerObject;
        private bool isPlayerSpawn = false;
        private ReactiveProperty<int> totalActiveEnemies = new ReactiveProperty<int>(0);
        private bool allEnemiesSpawned = false; // ウェーブ内の全ての敵がスポーンしたかどうかを示すフラグ
        public Camera mainCamera; // メインカメラ
        [SerializeField] private int maxActiveEnemies;

        /// <summary>
        /// ゲーム開始時に呼ばれ、ウェーブと敵の初期化
        /// </summary>
        public void Init(PlayerBase playerBase)
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
            cancelToken = new CancellationTokenSource();
            totalActiveEnemies
                .Where(activeEnemies => activeEnemies == 0)
                .Subscribe(async _ => await SpawnWave(cancelToken.Token));
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
        private async Task SpawnWave(CancellationToken token)
        {
            if(currentWaveIndex < waves.Length)
            {
                //Taskキャンセル時のエラー通知を消す処理
                var cancellationTask = new TaskCompletionSource<bool>();
                using (token.Register(() => cancellationTask.TrySetResult(true))) // usingスコープから外れたら自動的にオブジェクトをDispose();
                {
                    if (await Task.WhenAny(Task.Delay(TimeSpan.FromSeconds(3)), cancellationTask.Task) != cancellationTask.Task)
                    {
                        Wave currentWave = waves[currentWaveIndex];
                        int i = 0;
                        while(i < currentWave.totalEnemies)
                        {
                            if (token.IsCancellationRequested)
                            {
                                Debug.Log("task cancel.");
                                return;
                            }
                            if (totalActiveEnemies.Value >= maxActiveEnemies)
                            {
                                await Observable.EveryUpdate()
                                    .Where(_ => totalActiveEnemies.Value < maxActiveEnemies && !token.IsCancellationRequested)
                                    .First()
                                    .ToTask(token)
                                    .ConfigureAwait(false); // エラー通知を無効にする
                            }
                            int enemyIndex = GetRandomEnemyIndex(currentWave.enemies);
                            EnemyType chosenEnemy = currentWave.enemies[enemyIndex];
                            if (chosenEnemy.currentSpawnCount < chosenEnemy.maxSpawnCount)
                            {
                                SpawnEnemy(chosenEnemy);
                                chosenEnemy.currentSpawnCount++;
                                if (await Task.WhenAny(Task.Delay(2000), cancellationTask.Task) == cancellationTask.Task) // エネミーのスポーン間隔
                                {
                                    return;
                                }
                            }
                            i++;
                        }
                        await totalActiveEnemies
                            .Where(activeEnemies => activeEnemies == 0)
                            .First()
                            .ToTask(token);
                        currentWaveIndex++;
                        // 全ての敵がスポーンした後に遅延を入れてからフラグを更新
                        allEnemiesSpawned = true; // 全ての敵がスポーンしたのでフラグを更新
                    }
                }
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
            //敵の出現位置をランダムに設定
            //ここでスポーン位置を設定
            float spawnRadius = 15.0f;
            Vector3 playerPosition = playerObject.transform.position;
            Vector3 spawnOffset = new Vector3(
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),UnityEngine.Random.Range(-spawnRadius, spawnRadius),0
            );
            Vector3 spawnPosition = playerPosition + spawnOffset;
            Vector3 cameraPosition = mainCamera.transform.position;
            float distanceToCamera = Vector3.Distance(spawnPosition, cameraPosition);
            // カメラの範囲外のしきい値(範囲)を計算する
            float spawnThreshold = mainCamera.orthographicSize + spawnRadius;
            // カメラの範囲内に敵がいる場合、出現位置を調整する
            if (distanceToCamera < spawnThreshold)
            {
                spawnPosition += (spawnPosition - cameraPosition).normalized * (spawnThreshold - distanceToCamera);
            }
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
            if (allEnemiesSpawned && totalActiveEnemies.Value == 0)
            {
                Debug.Log("NextWave");
                waveAdvanceCount++;
                if(waveAdvanceCount == waveClearCount){
                    canvasShow.ClearCanvasShow();
                    SceneManager.LoadScene("ClearScene");
                }
                allEnemiesSpawned = false; // フラグをリセット
                await SpawnWave(cancelToken.Token);
            }
            enemyObject.SetActive(false);
            enemy.ResetSubscription();
        }
        /// <summary>
        /// ゲームオブジェクトが無効になったときに呼び出され、進行中のウェーブのスポーンをキャンセルする処理。
        /// </summary>
        void OnDisable()  // ゲーム停止時に呼び出されるメソッド
        {
            cancelToken.Cancel();  // 非同期タスクを停止する
        }
    }
}