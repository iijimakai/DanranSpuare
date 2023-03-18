#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

namespace CreateCode
{
    public class ObjectPoolCreateCode : EditorWindow
    {
        private string variableName = "ObjectPoolManager";
        private string createPath = "Assets/Scripts/";

        [MenuItem("CreateCode/ObjectPoolManager")]
        private static void ShowWindow()
        {
            ObjectPoolCreateCode window = GetWindow<ObjectPoolCreateCode>();
            window.titleContent = new GUIContent("CraeteMenu");
        }

        private void OnGUI()
        {
            GUILayout.Label("スクリプト名を入力してください");
            variableName = EditorGUILayout.TextField(variableName, GUILayout.Height(20));

            GUILayout.Label("以下の場所に生成されます");
            GUILayout.Label(createPath + variableName + ".cs");

            if (GUILayout.Button("スクリプト生成"))
            {
                CraeteCode();
            }
        }

        private void CraeteCode()
        {
            string filepath = createPath + variableName + ".cs";
            filepath = AssetDatabase.GenerateUniqueAssetPath(filepath);
            File.WriteAllText(filepath, SetCode());
            AssetDatabase.Refresh();
            Debug.Log(filepath);
        }

        private string SetCode()
        {
            string code = @"
using System.Collections.Generic;
using UnityEngine;

public class " + variableName + @" : MonoBehaviour
{
    [SerializeField,Header(""生成するオブジェクト"")]
    private GameObject createObj;

    private List<GameObject> objectPoolList = new();
    public void Create()
    {
        GameObject obj = Instantiate(createObj);
        obj.SetActive(false);
        obj.transform.parent = transform;
        objectPoolList.Add(obj);
    }

    public GameObject GetCreateObj(Transform parentobj)
    {
        GameObject obj = objectPoolList[0];
        obj.SetActive(true);
        obj.transform.parent = parentobj;
        objectPoolList.Remove(obj);
        return obj;
    }

    public void DeleteObj(GameObject obj)
    {
        objectPoolList.Add(obj);
        obj.SetActive(false);
        obj.transform.parent = transform;
    }
}";

            return code;
        }
    }
}
#endif