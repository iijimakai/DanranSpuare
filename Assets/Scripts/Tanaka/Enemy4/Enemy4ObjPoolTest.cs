using System.Collections.Generic;
using UnityEngine;
namespace pool
{
public class Enemy4ObjPoolTest : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int poolSize = 5;

    private List<GameObject> pooledObjects;

    private void Awake()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject();
        }
    }

    public GameObject GetObject()
    {
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = CreateNewObject();
        newObj.SetActive(true);
        return newObj;
    }

    private GameObject CreateNewObject()
    {
        GameObject newObj = Instantiate(objectPrefab);
        newObj.transform.SetParent(transform);
        newObj.SetActive(false);
        pooledObjects.Add(newObj);
        return newObj;
    }
}

}
