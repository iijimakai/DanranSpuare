using UnityEngine;
using UnityEditor;
using System.IO;

namespace CreateCode
{
    public enum CSVvariableType
    {
        None = 0,
        GetInt = 1 << 0,
        GetString = 1 << 1,
        GetFloat = 1 << 2,
    }

    public class CSVImporterCreate : EditorWindow
    {
        private string codeName = "CSVImporter";
        private string createPath = "Assets/Scripts/";

        private CSVvariableType variable;

        [MenuItem("CreateCode/CSVImporter"), MenuItem("Assets/CreateCode/CSVImporter")]
        private static void ShowWindow()
        {
            CSVImporterCreate window = GetWindow<CSVImporterCreate>();
            window.titleContent = new GUIContent("CreateMenu");
        }

        private void OnGUI()
        {
            codeName = EditorGUILayout.TextField(codeName);

            GUILayout.Label("以下の場所に生成されます");
            GUILayout.Label(createPath + codeName + ".cs");

            variable = (CSVvariableType)EditorGUILayout.EnumFlagsField("Getメソッド関数type", variable);

            GUILayout.Label("UniRxとAddresableAssetを入れないとエラーが出ます");

            if (GUILayout.Button("スクリプト生成"))
            {
                CreateCode();
            }
        }

        private void CreateCode()
        {
            string filePath = createPath + codeName + ".cs";
            filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
            File.WriteAllText(filePath, SetCode());
            AssetDatabase.Refresh();
            Debug.Log("生成終了しました");
        }

        private string SetCode()
        {
            string code =
                @"using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;
using System;
using UnityEditor;

public class " + codeName + @"
{
    private TextAsset csv;

    private List<List<string>> csvData = new(50);

    private Subject<bool> endCSVRead = new Subject<bool>();


    public void ReadCSV(string filePath)
    {
        csv = null;
        var load = Addressables.LoadAssetAsync<TextAsset>(filePath);
        csv = load.WaitForCompletion();

        string path = AssetDatabase.GetAssetPath(csv);

        StreamReader sr = new StreamReader(path);
        
        while (sr.Peek() > -1)
        {
            string line = sr.ReadLine();
            string[] value = line.Split(',');

            List<string> lists = new List<string>(30);
            lists.AddRange(value);

            csvData.Add(lists);

            for(int i = 0; i < value.Length; i++)
            {
                Debug.Log(value[i]);
            }
        }

        sr.Close();
        endCSVRead.OnNext(true);
    }" 
+ GetMethodCreate() +
    @"
    public IObservable<bool> GetEndCSVRead()
    {
        return endCSVRead;
    }   
}";

            return code;
        }

        private string GetMethodCreate()
        {
            string methodData = "";

            if(((int)variable & 1 << 0) != 0)
            {
                methodData += @"
    public List<int> GetInt(int callNum)
    {
        List<int> returnList = new List<int>(30);

        for(int i = 0; i < csvData.Count; i++)
        {
            returnList.Add(int.Parse(csvData[i][callNum]));
        }
        return returnList;
    }";
            }
            if(((int)variable & 1 << 1) != 0)
            {
                methodData += @"
    public List<string> GetString(int callNum)
    {
        List<string> returnList = new List<string>(30);

        for(int i = 0; i < csvData.Count; i++)
        {
            returnList.Add(csvData[i][callNum]);
        }
        return returnList;
    }";
            }
            if(((int)variable & 1 << 2) != 0)
            {
                methodData += @"
    public List<float> GetFloat(int callNum)
    {
        List<float> returnList = new List<float>(30);

        for(int i = 0; i < csvData.Count; i++)
        {
            returnList.Add(float.Parse(csvData[i][callNum]));
        }
        return returnList;
    }";
            }
            return methodData;
        }
    }
}