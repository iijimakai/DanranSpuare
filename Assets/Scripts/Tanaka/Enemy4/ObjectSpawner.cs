using System.Collections;
using UnityEngine;
using pool;
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Enemy4ObjPoolTest objectPool;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 1f;

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject obj = objectPool.GetObject();
            obj.transform.position = spawnPoint.position;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
