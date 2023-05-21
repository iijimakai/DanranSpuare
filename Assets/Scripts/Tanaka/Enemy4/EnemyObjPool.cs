using System.Collections.Generic;
using UnityEngine;

namespace pool
{
    // 敵のオブジェクトプールを管理するクラス
    public class EnemyObjPool : MonoBehaviour
    {
        [SerializeField]
        // プールする敵のプレハブ
        private GameObject objectPrefab;
        // プールする敵の最大数
        private int poolSize;
        // プールされた敵のリスト
        private List<GameObject> pooledObjects;

        /// <summary>
        /// 敵のプールを初期化する
        /// </summary>
        /// <param name="prefab">プールする敵のプレハブ</param>
        /// <param name="size">プールする敵の最大数</param>
        public void Initialize(GameObject prefab, int size)
        {
            objectPrefab = prefab;
            //Debug.Log("Initializing pool with prefab: " + (objectPrefab != null ? objectPrefab.name : "null"));
            poolSize = size;
            pooledObjects = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                CreateNewObject();
            }
        }
        /// <summary>
        /// アクティブでない敵をプールから取得する
        /// </summary>
        /// <returns>アクティブでない敵のGameObject</returns>
        public GameObject GetObject()
        {
            foreach (var obj in pooledObjects)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    obj.transform.position = Vector3.zero;  // 座標をリセット
                    obj.transform.rotation = Quaternion.identity;  // 回転をリセット
                    return obj;
                }
            }
            //Debug.Log("Finish check");
            GameObject newObj = CreateNewObject();
            newObj.SetActive(true);
            newObj.transform.position = Vector3.zero;  // 座標をリセット
            newObj.transform.rotation = Quaternion.identity;  // 回転をリセット
            return newObj;
        }
        /// <summary>
        /// 新しい敵のGameObjectを作成し、プールに追加する
        /// </summary>
        /// <returns>作成された敵のGameObject</returns>
        private GameObject CreateNewObject()
        {
            //Debug.Log("Creating new object from prefab: " + (objectPrefab != null ? objectPrefab.name : "null"));
            GameObject newObj = Instantiate(objectPrefab);
            newObj.transform.SetParent(transform);
            newObj.SetActive(false);
            pooledObjects.Add(newObj);
            //Debug.Log("Created new object: " + newObj.name);
            return newObj;
        }
    }
}
