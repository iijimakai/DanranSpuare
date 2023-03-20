using System.Collections.Generic;
using UnityEngine;

namespace Sora_System
{
    public class ObjectPoolBase : MonoBehaviour
    {
        private List<GameObject> objectPoolList = new();

        /// <summary>
        /// Poolするアイテムの生成
        /// </summary>
        /// <param name="createObj">Poolするオブジェクト</param>
        public void Create(GameObject createObj)
        {
            GameObject obj = Instantiate(createObj);
            obj.SetActive(false);
            obj.transform.parent = transform;
            objectPoolList.Add(obj);
        }

        /// <summary>
        /// Poolのアイテムを取り出す
        /// </summary>
        /// <param name="parentobj">取り出し場所</param>
        /// <returns>Poolしたオブジェクト</returns>
        public GameObject GetCreateObj(Transform parentobj)
        {
            GameObject obj = objectPoolList[0];
            obj.SetActive(true);
            obj.transform.position = parentobj.position;
            objectPoolList.Remove(obj);
            return obj;
        }

        /// <summary>
        /// Poolのオブジェクトをしまう
        /// </summary>
        /// <param name="obj">Poolのオブジェクト</param>
        public void DeleteObj(GameObject obj)
        {
            objectPoolList.Add(obj);
            obj.SetActive(false);
            obj.transform.parent = transform;
        }
    }
}