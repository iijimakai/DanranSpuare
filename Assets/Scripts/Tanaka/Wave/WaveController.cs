using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using pool;
using System.Linq;

public class WaveController : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab; //敵のプレハブ
        public float spawnChance; //出現確率
        public int maxSpawnCount; //最大出現数
        [HideInInspector] public int currentSpawnCount; //現在の出現数
    }

    [System.Serializable]
    public class Wave
    {
        public int totalEnemies; //Wave内の敵の合計数
        public EnemyType[] enemies; //Wave内で出現可能な敵の配列
    }

    public Wave[] waves; //Waveの配列
    private int currentWaveIndex = 0; //現在のWaveのインデックス
    [SerializeField] private int poolSize = 5; // プールサイズ
    private Dictionary<GameObject, EnemyObjPool> enemyPools = new Dictionary<GameObject, EnemyObjPool>(); //敵のプレハブごとのオブジェクトプール


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
                enemyType.currentSpawnCount = 0;
            }
        }

        Observable.NextFrame().Subscribe(_ => SpawnWave());
    }


    private void SpawnWave()
    {
        if(currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            int[] spawnCount = new int[currentWave.enemies.Length];
            for (int i = 0; i < currentWave.totalEnemies; i++)
            {
                int enemyIndex = GetRandomEnemyIndex(currentWave.enemies);
                if(spawnCount[enemyIndex] < currentWave.enemies[enemyIndex].maxSpawnCount)
                {
                    SpawnEnemy(currentWave.enemies[enemyIndex]);
                    spawnCount[enemyIndex]++;
                }
                else
                {
                    i--;
                }
            }

            currentWaveIndex++;
        }
    }

    private int GetRandomEnemyIndex(EnemyType[] enemies)
    {
        float total = 0;
        float randomPoint = Random.value;

        for (int i= 0; i< enemies.Length; i++)
        {
            total += enemies[i].spawnChance;
            if (randomPoint <= total)
            {
                return i;
            }
        }

        return enemies.Length - 1;
    }

    private void SpawnEnemy(EnemyType enemyType)
    {
        Debug.Log("Trying to spawn enemy: " + enemyType.enemyPrefab.name);
        GameObject spawnedEnemyObject = enemyPools[enemyType.enemyPrefab].GetObject(); // プールから敵を取得
        Debug.Log("Got enemy object from pool: " + spawnedEnemyObject.name);

        EnemySystemtest spawnedEnemy = spawnedEnemyObject.GetComponent<EnemySystemtest>();
        Debug.Log("Got EnemySystemtest from enemy object: " + spawnedEnemy);

        //敵の出現位置をランダムに設定
        //ここでスポーン位置を設定

        //敵が破壊されたときにプールに戻るように設定
        spawnedEnemy.OnDestroyed.Subscribe(_ =>
        {
            spawnedEnemyObject.SetActive(false);
            if (AllEnemiesDestroyed())
            {
                SpawnWave();
            }
        });
    }

    private bool AllEnemiesDestroyed()
    {
        foreach (var pool in enemyPools.Values)
        {
            if (pool.GetComponentsInChildren<Transform>(true).Any(t => t.gameObject.activeSelf))
            {
                return false;
            }
        }
        return true;
    }
}

