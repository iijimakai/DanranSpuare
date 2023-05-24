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
        // 現在アクティブな敵の数を記録するカウンター
        private int totalActiveEnemies = 0;
        int tempCount = 0;

        private int waveCurrentAppearanceLimit = 0;
        //int waveEnemySpawnMaxCount = 0;
        /// <summary>
        /// 最初のウェーブを生成。
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
            Observable.NextFrame().Subscribe(_ => SpawnWave(cancelToken.Token).Forget());
        }

        /// <summary>
        /// 敵のウェーブを生成。
        /// </summary>
private async UniTaskVoid SpawnWave(CancellationToken token)
{
    if(currentWaveIndex < waves.Length)
    {
        if(waveCurrentAppearanceLimit < 20)
        {
            Wave currentWave = waves[currentWaveIndex];
            for(int i = 0; i < currentWave.totalEnemies; i++)
            {
                int enemyIndex = GetRandomEnemyIndex(currentWave.enemies);
                EnemyType chosenEnemy = currentWave.enemies[enemyIndex];
                if (chosenEnemy.currentSpawnCount < chosenEnemy.maxSpawnCount)
                {
                    SpawnEnemy(chosenEnemy);
                    chosenEnemy.currentSpawnCount++;
                    waveCurrentAppearanceLimit++;
                    Debug.Log("appearancelimit" + waveCurrentAppearanceLimit);
                    await UniTask.Delay(2000, cancellationToken: token);

                    token.ThrowIfCancellationRequested();
                }
                else
                {
                    i--;
                }
            }
        }
        currentWaveIndex++;

        await UniTask.Delay(2000, cancellationToken: token);

        token.ThrowIfCancellationRequested();
    }
}        /// <summary>
        /// 生成する敵のたいぷのインデックスを、その生成確率に基づいてかえす。
        /// </summary>
        /// <param name="enemies">生成可能な敵のタイプの配列。</param>
        /// <returns>生成する敵のタイプのインデックス。</returns>
        private int GetRandomEnemyIndex(EnemyType[] enemies)
        {
            float total = 0;
            float randomPoint = Random.value;

            for (int i = 0; i < enemies.Length; i++)
            {
                total += enemies[i].spawnChance;
                if (randomPoint <= total)
                {
                    Debug.Log(i);
                    return i;
                }
            }
            throw new System.Exception("a");
            //return enemies.Length - 1;
        }
        /// <summary>
        /// 指定したタイプの敵を1体生成する。
        /// </summary>
        /// <param name="enemyType">生成する敵のタイプ。</param>
        private void SpawnEnemy(EnemyType enemyType)
        {
            GameObject spawnedEnemyObject = enemyPools[enemyType.enemyPrefab].GetObject(); // プールから敵を取得
            if(spawnedEnemyObject == null)
            {
                // エラーログを出力するなど、適切な処理を行う。
                Debug.LogError("Enemy object is null.");
                return;
            }
            IEnemy spawnedEnemy = spawnedEnemyObject.GetComponent<IEnemy>();
            totalActiveEnemies++;
            //敵の出現位置をランダムに設定
            //ここでスポーン位置を設定
            float spawnRadius = 15.0f;
            Vector3 playerPosition = playerObject.transform.position;
            Vector3 spawnPosition = playerPosition + new Vector3(
                Random.Range(-spawnRadius,spawnRadius),
                Random.Range(-spawnRadius,spawnRadius),
                Random.Range(-spawnRadius,spawnRadius)
            );
            spawnedEnemyObject.transform.position = spawnPosition;
            //敵が破壊されたときにプールに戻るように設定
            spawnedEnemy.OnDestroyed.Subscribe(_ =>
            {
                // 敵が破壊されたので、カウンターを減らす
                totalActiveEnemies--;
                spawnedEnemyObject.SetActive(false);
                tempCount++;
                Debug.Log("DeathCount" + tempCount);
                if (totalActiveEnemies == 0)
                {
                    Debug.Log("All Enemies Destroy");
                    Debug.Log("NextWave");
                    SpawnWave(cancelToken.Token).Forget(); // UniTaskを呼び出し、その結果を破棄
                }
                spawnedEnemy.ResetSubscription();
            }).AddTo(this);
        }
        void OnDisable()  // ゲーム停止時に呼び出されるメソッド
        {
            cancelToken.Cancel();  // 非同期タスクを停止する
        }
    }

}


