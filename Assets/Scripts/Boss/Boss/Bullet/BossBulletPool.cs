using System.Collections.Generic;
using UnityEngine;

namespace pool
{
    // 敵のオブジェクトプールを管理するクラス
    public class BossBulletPool : MonoBehaviour
    {
        [SerializeField] private GameObject objectPrefab; // プールするプレハブ
        // プールするオブジェクトの最大数
        private int poolSize;
        // プールされたオブジェクトのリスト
        private List<GameObject> pooledObjects;
        public static BossBulletPool Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            pooledObjects = new List<GameObject>();
        }
        /// <summary>
        /// 敵のプールを初期化する
        /// </summary>
        /// <param name="prefab">プールするオブジェクトのプレハブ</param>
        /// <param name="size">プールするオブジェクトの最大数</param>
        public void Initialize(GameObject prefab, int size)
        {
            objectPrefab = prefab;
            poolSize = size;
            pooledObjects = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                CreateNewObject();
            }
        }
        /// <summary>
        /// アクティブでないオブジェクトをプールから取得する
        /// </summary>
        /// <returns>アクティブでないオブジェクト</returns>
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
            GameObject newObj = CreateNewObject();
            newObj.SetActive(true);
            newObj.transform.position = Vector3.zero;  // 座標をリセット
            newObj.transform.rotation = Quaternion.identity;  // 回転をリセット
            return newObj;
        }
        /// <summary>
        /// 新しいオブジェクトを作成し、プールに追加する
        /// </summary>
        /// <returns>作成された敵のオブジェクト</returns>
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
