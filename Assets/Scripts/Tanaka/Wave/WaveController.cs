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
    }

    [System.Serializable]
    public class Wave
    {
        public int totalEnemies; //Wave内の敵の合計数
        public EnemyType[] enemies; //Wave内で出現可能な敵の配列
    }

    public Wave[] waves; //Waveの配列
    private int currentWaveIndex = 0; //現在のWaveのインデックス

    private Dictionary<GameObject, EnemyObjPool> enemyPools = new Dictionary<GameObject, EnemyObjPool>(); //敵のプレハブごとのオブジェクトプール

    void Start()
    {
        foreach(var wave in waves)
        {
            foreach(var enemyType in wave.enemies)
            {
                var pool = new GameObject(enemyType.enemyPrefab.name + " Pool").AddComponent<EnemyObjPool>();
                pool.SetPrefab(enemyType.enemyPrefab);
                enemyPools[enemyType.enemyPrefab] = pool;
            }
        }

        SpawnWave();
    }

    private void SpawnWave()
    {
        if(currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            int[] spawnCount = new int[currentWave.enemies.Length];

            for(int i = 0; i < currentWave.totalEnemies; i++)
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
        GameObject spawnedEnemyObject = enemyPools[enemyType.enemyPrefab].GetObject(); // プールから敵を取得
        EnemySystemtest spawnedEnemy = spawnedEnemyObject.GetComponent<EnemySystemtest>();

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

