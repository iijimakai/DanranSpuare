using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using pool;
using System.Linq;
using System.Threading.Tasks;
namespace wave
{
    public class WaveController : MonoBehaviour
    {
        [System.Serializable]
        public class EnemyType
        {
            [SerializeField, Header("敵の種類を設定")]
            public GameObject enemyPrefab; //敵のプレハブ
            [SerializeField, Header("出現確率")]
            public float spawnChance; //出現確率
            [SerializeField, Header("出現上限")]
            public int maxSpawnCount; //最大出現数
            [SerializeField, Header("現在の出現数")]
            [HideInInspector]
            public int currentSpawnCount; //現在の出現数
        }

        [System.Serializable]
        public class Wave
        {
            [SerializeField, Header("このWaveでの敵の合計数")]
            public int totalEnemies; //Waveの敵の合計数
            [SerializeField, Header("Waveで出現可能な敵の種類の数")]
            public EnemyType[] enemies; //Waveで出現可能な敵の種類の配列
        }
        [SerializeField, Header("Wave数")]
        public Wave[] waves; //Waveの配列
        private int currentWaveIndex = 0; //現在のWaveのインデックス
        [SerializeField] private int poolSize = 5; // プールサイズ
        private Dictionary<GameObject, EnemyObjPool> enemyPools = new Dictionary<GameObject, EnemyObjPool>(); //敵のプレハブごとのオブジェクトプール

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
            Observable.NextFrame().Subscribe(_ => SpawnWave());
        }

        /// <summary>
        /// 敵のウェーブを生成。
        /// </summary>
        private async void SpawnWave()
        {
            if(currentWaveIndex < waves.Length)
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
                        await Task.Delay(2000);
                    }
                    else
                    {
                        i--;
                    }
                }
                currentWaveIndex++;

                await Task.Delay(2000);

            }
        }
        /// <summary>
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
            Debug.Log(enemyPools.ContainsKey(enemyType.enemyPrefab)); // enemyPoolsにenemyPrefabのキーが存在するか確認
            Debug.Log(enemyPools[enemyType.enemyPrefab]); // enemyPools[enemyType.enemyPrefab]自体がnullでないことを確認
            GameObject spawnedEnemyObject = enemyPools[enemyType.enemyPrefab].GetObject(); // プールから敵を取得
            //Debug.Log(spawnedEnemyObject.name);
            if(spawnedEnemyObject == null)
            {
                // エラーログを出力するなど、適切な処理を行う。
                Debug.LogError("Enemy object is null.");
                return;
            }
            Enemy4Controller spawnedEnemy = spawnedEnemyObject.GetComponent<Enemy4Controller>();
            //Debug.Log(spawnedEnemy);
            spawnedEnemy.SubscriptionReset();
            //敵の出現位置をランダムに設定
            //ここでスポーン位置を設定

            //敵が破壊されたときにプールに戻るように設定
            spawnedEnemy.OnDestroyed.Subscribe(_ =>
            {
                //Debug.Log("OnDestroyed" + spawnedEnemyObject.name);
                spawnedEnemyObject.SetActive(false);
                //Debug.Log("Is active: " + spawnedEnemyObject.activeSelf);
                if (CheckAllEnemiesDestroyed())
                {
                    Debug.Log("SpawnWave");
                    SpawnWave();
                }
            });
        }
        /// <summary>
        /// すべての敵が破壊されたかどうかを確認する。
        /// </summary>
        /// <returns>すべての敵が破壊されていればtrue、そうじゃなかったらfalseを返す。</returns>
        private bool CheckAllEnemiesDestroyed()
        {
            foreach (var pool in enemyPools.Values)
            {
                var activeEnemies = pool.GetComponentsInChildren<Enemy4Controller>(true)
                .Where(t => t.gameObject.activeSelf);
                foreach(var activeEnemy in activeEnemies)
                {
                    //Debug.Log("Active enemy in pool: " + activeEnemy.name);
                }
                if(activeEnemies.Any())
                {
                    return false;
                }
            }
            Debug.Log("All Enemies Destroy");
            return true;
        }
    }

}


