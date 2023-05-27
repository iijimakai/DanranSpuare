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

namespace wave
{
    public class WaveController : MonoBehaviour
    {
        [System.Serializable]
        public class EnemyType
        {
            [SerializeField, Header("敵の種類を設定")]
            public GameObject enemyPrefab; // 敵のプレハブ
            [SerializeField, Header("出現確率")]
            public float spawnChance; // 出現確率
            [SerializeField, Header("出現上限")]
            public int maxSpawnCount; // 最大出現数
            [SerializeField, Header("現在の出現数")]
            [HideInInspector]
            public int currentSpawnCount; // 現在の出現数
        }

        [System.Serializable]
        public class Wave
        {
            [SerializeField, Header("このWaveでの敵の合計数")]
            public int totalEnemies; // Waveの敵の合計数
            [SerializeField, Header("Waveで出現可能な敵の種類の数")]
            public EnemyType[] enemies; // Waveで出現可能な敵の種類の配列
        }
        [SerializeField, Header("Wave数")]
        public Wave[] waves; // Waveの配列
        private int currentWaveIndex = 0; // 現在のWaveのインデックス
        [SerializeField] private int poolSize = 5; // プールサイズ
        private Dictionary<GameObject, EnemyObjPool> enemyPools = new Dictionary<GameObject, EnemyObjPool>(); //敵のプレハブごとのオブジェクトプール
        private CancellationTokenSource cancelToken;
        private CompositeDisposable enemySubscriptions = new CompositeDisposable();
        [SerializeField] private PlayerInputTest playerObject;
        private ReactiveProperty<int> totalActiveEnemies = new ReactiveProperty<int>(0);
        private bool allEnemiesSpawned = false; // 追加: ウェーブ内の全ての敵がスポーンしたかどうかを示すフラグ
        int tempCount = 0;

        [SerializeField]
        private int maxActiveEnemies;
        /// <summary>
        /// ゲーム開始時に呼ばれ、ウェーブと敵の初期化を行います。
        /// </summary>
        void Start()
        {
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
                    // 初期化
                    //enemyType.currentSpawnCount = 0;
                }
            }
            cancelToken = new CancellationTokenSource();
            totalActiveEnemies
                .Where(activeEnemies => activeEnemies == 0)
                .Subscribe(async _ => await SpawnWave(cancelToken.Token));
                //Observable.NextFrame().Subscribe(async _ => await SpawnWave(cancelToken.Token));
        }
        /// <summary>
        /// 指定されたウェーブの敵をスポーン。
        /// </summary>
        /// <param name="token">非同期タスクのキャンセルトークン</param>
        /// <returns>ウェーブのスポーンが完了したら完了するタスク</returns>
        private async Task SpawnWave(CancellationToken token)
        {
            if(currentWaveIndex < waves.Length)
            {
                // 新しいWaveの敵が生成される前に一定時間の遅延を導入する
                await Task.Delay(TimeSpan.FromSeconds(3), token);
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
                            .ToTask(token);
                            //.ConfigureAwait(false); // エラー通知を無効にする
                    }
                    int enemyIndex = GetRandomEnemyIndex(currentWave.enemies);
                    EnemyType chosenEnemy = currentWave.enemies[enemyIndex];
                    if (chosenEnemy.currentSpawnCount < chosenEnemy.maxSpawnCount)
                    {
                        SpawnEnemy(chosenEnemy);
                        chosenEnemy.currentSpawnCount++;
                        await Task.Delay(2000, token);//.ConfigureAwait(false); // エラー通知を無効にする
                    }
                    i++;
                }
                await totalActiveEnemies
                    .Where(activeEnemies => activeEnemies == 0)
                    .First()
                    .ToTask(token);
                //allEnemiesSpawned = false; // フラグをリセット
                currentWaveIndex++;
                Debug.Log(currentWaveIndex);
                // 全ての敵がスポーンした後に遅延を入れてからフラグを更新
                allEnemiesSpawned = true; // 全ての敵がスポーンしたのでフラグを更新
                Debug.Log("allEnemiesSpawned;"+allEnemiesSpawned);
            }
        }

        /// <summary>
        /// 敵のタイプのインデックスをランダムに選択。
        /// 選択は敵のスポーン確率に基づく。
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
                    //Debug.Log(i);
                    return i;
                }
            }
            throw new System.Exception("a");
        }
        /// <summary>
        /// 指定されたタイプの敵をスポーンします。
        /// </summary>
        /// <param name="enemyType">スポーンさせる敵のタイプ</param>
        private void SpawnEnemy(EnemyType enemyType)
        {
            GameObject spawnedEnemyObject = enemyPools[enemyType.enemyPrefab].GetObject(); // プールから敵を取得
            if(spawnedEnemyObject == null)
            {
                // エラーログを出力するなど、適切な処理を行う。
                Debug.LogError("Enemy object is null.");
                return;
            }
            totalActiveEnemies.Value++;
            Debug.Log("Total Active Enemies after spawn: " + totalActiveEnemies.Value);
            IEnemy spawnedEnemy = spawnedEnemyObject.GetComponent<IEnemy>();
            //敵の出現位置をランダムに設定
            //ここでスポーン位置を設定
            float spawnRadius = 15.0f;
            Vector3 playerPosition = playerObject.transform.position;
            Vector3 spawnPosition = playerPosition + new Vector3(
                UnityEngine.Random.Range(-spawnRadius,spawnRadius),
                UnityEngine.Random.Range(-spawnRadius,spawnRadius),
                UnityEngine.Random.Range(-spawnRadius,spawnRadius)
            );
            spawnedEnemyObject.transform.position = spawnPosition;
            //敵が破壊されたときにプールに戻るように設定
            spawnedEnemy.OnDestroyed.Subscribe(async _ =>
            {
                DestroyEnemy(spawnedEnemyObject, spawnedEnemy);
            }).AddTo(this);
        }
            /// <summary>
        /// 指定された敵オブジェクトを破壊します。
        /// </summary>
        /// <param name="enemyObject">破壊する敵オブジェクト</param>
        /// <param name="enemy">破壊する敵</param>
        private async void DestroyEnemy(GameObject enemyObject, IEnemy enemy)
        {
            totalActiveEnemies.Value--;
            Debug.Log("Total Active Enemies after destroy: " + totalActiveEnemies.Value);
            //
            if (allEnemiesSpawned && totalActiveEnemies.Value == 0)
            {
                Debug.Log("All Enemies Destroy");
                Debug.Log("NextWave");
                allEnemiesSpawned = false; // フラグをリセット
                Debug.Log("allEnemiesSpawned" + allEnemiesSpawned);
                await SpawnWave(cancelToken.Token);
            }
            enemyObject.SetActive(false);
            tempCount++;
            Debug.Log("DeathCount" + tempCount);
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


