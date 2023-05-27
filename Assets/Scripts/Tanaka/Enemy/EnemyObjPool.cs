using System.Collections.Generic;
using UnityEngine;

namespace pool
{
    // �G�̃I�u�W�F�N�g�v�[�����Ǘ�����N���X
    public class EnemyObjPool : MonoBehaviour
    {
        [SerializeField]
        // �v�[������G�̃v���n�u
        private GameObject objectPrefab;
        // �v�[������G�̍ő吔
        private int poolSize;
        // �v�[�����ꂽ�G�̃��X�g
        private List<GameObject> pooledObjects;

        /// <summary>
        /// �G�̃v�[��������������
        /// </summary>
        /// <param name="prefab">�v�[������G�̃v���n�u</param>
        /// <param name="size">�v�[������G�̍ő吔</param>
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
        /// �A�N�e�B�u�łȂ��G���v�[������擾����
        /// </summary>
        /// <returns>�A�N�e�B�u�łȂ��G��GameObject</returns>
        public GameObject GetObject()
        {
            foreach (var obj in pooledObjects)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    obj.transform.position = Vector3.zero;  // ���W�����Z�b�g
                    obj.transform.rotation = Quaternion.identity;  // ��]�����Z�b�g
                    return obj;
                }
            }
            //Debug.Log("Finish check");
            GameObject newObj = CreateNewObject();
            newObj.SetActive(true);
            newObj.transform.position = Vector3.zero;  // ���W�����Z�b�g
            newObj.transform.rotation = Quaternion.identity;  // ��]�����Z�b�g
            return newObj;
        }
        /// <summary>
        /// �V�����G��GameObject���쐬���A�v�[���ɒǉ�����
        /// </summary>
        /// <returns>�쐬���ꂽ�G��GameObject</returns>
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
