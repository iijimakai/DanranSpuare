#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace CreateCode
{
    public enum ModelvariableType
    {
        None = 0,
        Int = 1 << 0,
        String = 1 << 1,
        Floot = 1 << 2,
        Vector3 = 1 << 3,
        List_Int = 1 << 4,
        List_String = 1 << 5,
        List_Floot = 1 << 6,
        List_Vector3 = 1 << 7
    };

    public class ModelCodeCreate : EditorWindow
    {
        private string modelScriptName = "Model";
        private string presenterScriptName = "ModelPresenter";
        private string createPath = "Assets/Scripts/";

        private ModelvariableType variable;

        private int[] selectValue = new int[8];

        private string[] variableName = new string[100];

        private string[] exportVariableTypeName = { "int", "string", "float"
            , "Vector3", "List<int>", "List<string>" ,"List<float>","List<Vector3>"};

        private Vector2 scrollpositon = new();

        [MenuItem("CreateCode/Model&Presenter")]
        private static void DisplayWindow()
        {
            ModelCodeCreate window = GetWindow<ModelCodeCreate>();
            window.titleContent = new GUIContent("CreateMenu");
        }

        private void OnGUI()
        {
            GUILayout.Label("Modelのスクリプト名を入力してください");
            modelScriptName = EditorGUILayout.TextField(modelScriptName, GUILayout.Height(20));

            GUILayout.Label("Presenterのスクリプト名を入力してください");
            presenterScriptName = EditorGUILayout.TextField(presenterScriptName, GUILayout.Height(20));

            GUILayout.Label("以下の場所に生成されます");
            GUILayout.Label("ModelScript：" + createPath + modelScriptName + ".cs");
            GUILayout.Label("Presenter：" + createPath + presenterScriptName + ".cs");

            //enumをポップアップで複数選択
            variable = (ModelvariableType)EditorGUILayout.EnumFlagsField("変数タイプ", variable);

            //生成数の調整
            #region
            GUILayout.Label("変数の生成数");
            using (new EditorGUILayout.HorizontalScope())
            {
                if (((int)variable & 1 << 0) != 0)
                {
                    GUILayout.Label("Int");
                    selectValue[0] = EditorGUILayout.IntField(selectValue[0], GUILayout.Height(20), GUILayout.Width(30));
                }
                if (((int)variable & 1 << 1) != 0)
                {
                    GUILayout.Label("String");
                    selectValue[1] = EditorGUILayout.IntField(selectValue[1], GUILayout.Height(20), GUILayout.Width(30));
                }
                if (((int)variable & 1 << 2) != 0)
                {
                    GUILayout.Label("Floot");
                    selectValue[2] = EditorGUILayout.IntField(selectValue[2], GUILayout.Height(20), GUILayout.Width(30));
                }
                if (((int)variable & 1 << 3) != 0)
                {
                    GUILayout.Label("Vector3");
                    selectValue[3] = EditorGUILayout.IntField(selectValue[3], GUILayout.Height(20), GUILayout.Width(30));
                }
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                if (((int)variable & 1 << 4) != 0)
                {
                    GUILayout.Label("List_Int");
                    selectValue[4] = EditorGUILayout.IntField(selectValue[4], GUILayout.Height(20), GUILayout.Width(30));
                }
                if (((int)variable & 1 << 5) != 0)
                {

                    GUILayout.Label("List_String");
                    selectValue[5] = EditorGUILayout.IntField(selectValue[5], GUILayout.Height(20), GUILayout.Width(30));
                }
                if (((int)variable & 1 << 6) != 0)
                {

                    GUILayout.Label("List_Floot");
                    selectValue[6] = EditorGUILayout.IntField(selectValue[6], GUILayout.Height(20), GUILayout.Width(30));
                }
                if (((int)variable & 1 << 7) != 0)
                {

                    GUILayout.Label("List_Vector3");
                    selectValue[7] = EditorGUILayout.IntField(selectValue[7], GUILayout.Height(20), GUILayout.Width(30));
                }
            }
            #endregion

            //変数の名前
            #region
            GUILayout.Label("変数の名前");
            int addnumber = 0;

            scrollpositon = EditorGUILayout.BeginScrollView(scrollpositon);

            if (((int)variable & 1 << 0) != 0)
            {
                for (int i = 1; i <= selectValue[0]; i++)
                {
                    GUILayout.Label("Int_" + i);
                    variableName[addnumber] = EditorGUILayout.TextField(variableName[addnumber]
                        , GUILayout.Height(20));
                    addnumber++;
                }
            }
            if (((int)variable & 1 << 1) != 0)
            {
                for (int i = 1; i <= selectValue[1]; i++)
                {
                    GUILayout.Label("String_" + i);
                    variableName[addnumber] = EditorGUILayout.TextField(variableName[addnumber]
                        , GUILayout.Height(20));
                    addnumber++;
                }
            }
            if (((int)variable & 1 << 2) != 0)
            {
                for (int i = 1; i <= selectValue[2]; i++)
                {
                    GUILayout.Label("Floot_" + i);
                    variableName[addnumber] = EditorGUILayout.TextField(variableName[addnumber]
                        , GUILayout.Height(20));
                    addnumber++;
                }
            }
            if (((int)variable & 1 << 3) != 0)
            {
                for (int i = 1; i <= selectValue[3]; i++)
                {
                    GUILayout.Label("Vector3_" + i);
                    variableName[addnumber] = EditorGUILayout.TextField(variableName[addnumber]
                        , GUILayout.Height(20));
                    addnumber++;
                }
            }
            if (((int)variable & 1 << 4) != 0)
            {
                for (int i = 1; i <= selectValue[4]; i++)
                {
                    GUILayout.Label("List_Int_" + i);
                    variableName[addnumber] = EditorGUILayout.TextField(variableName[addnumber]
                        , GUILayout.Height(20));
                    addnumber++;
                }
            }
            if (((int)variable & 1 << 5) != 0)
            {
                for (int i = 1; i <= selectValue[5]; i++)
                {
                    GUILayout.Label("List_String_" + i);
                    variableName[addnumber] = EditorGUILayout.TextField(variableName[addnumber]
                        , GUILayout.Height(20));
                    addnumber++;
                }
            }
            if (((int)variable & 1 << 6) != 0)
            {
                for (int i = 1; i <= selectValue[6]; i++)
                {
                    GUILayout.Label("List_Floot_" + i);
                    variableName[addnumber] = EditorGUILayout.TextField(variableName[addnumber]
                        , GUILayout.Height(20));
                    addnumber++;
                }
            }
            if (((int)variable & 1 << 7) != 0)
            {
                for (int i = 1; i <= selectValue[7]; i++)
                {
                    GUILayout.Label("List_Vector3_" + i);
                    variableName[addnumber] = EditorGUILayout.TextField(variableName[addnumber]
                        , GUILayout.Height(20));
                    addnumber++;
                }
            }

            EditorGUILayout.EndScrollView();

            #endregion

            if (CheckVariableName(addnumber))
            {
                GUILayout.Label("UniRxがないとエラーが出ます。");
                if (GUILayout.Button("スクリプト生成"))
                {
                    string filepath = createPath + modelScriptName + ".cs";
                    filepath = AssetDatabase.GenerateUniqueAssetPath(filepath);
                    File.WriteAllText(filepath, CreateModelCode());
                    AssetDatabase.Refresh();
                    Debug.Log(modelScriptName + "を生成しました");
                    filepath = createPath + presenterScriptName + ".cs";
                    filepath = AssetDatabase.GenerateUniqueAssetPath(filepath);
                    File.WriteAllText(filepath, CreatePresenterCode());
                    AssetDatabase.Refresh();
                    Debug.Log(presenterScriptName + "を生成しました");
                }
            }
            else
            {
                GUILayout.Label("変数名を全部入れないと実行できません");
            }

        }

        private bool CheckVariableName(int adddnum)
        {
            for (int i = 0; i < adddnum; i++)
            {
                if (variableName[i] == " " || variableName[i] == "" || variableName[i] == null)
                {
                    return false;
                }
            }

            return true;
        }

        private string CreateModelCode()
        {
            string returnModeleCode = "";

            int addnum = 0;

            List<string> exportvariable = new();

            List<string> setFuction = new();
            List<string> getFuction = new();

            for (int i = 0; i < 8; i++)
            {
                if (((int)variable & 1 << i) != 0)
                {
                    for (int j = 1; j <= selectValue[0]; j++)
                    {
                        exportvariable.Add(exportVariableTypeName[i] + " " + char.ToLower(variableName[addnum][0])
                            + variableName[addnum].Substring(1) + ";\r\n\r\n");
                        setFuction.Add("\tpublic void Set" + char.ToUpper(variableName[addnum][0])
                            + variableName[addnum].Substring(1) + "(" + exportVariableTypeName[i] + " getVariable)\r\n\t{\r\n\t\t"
                            + variableName[addnum] + " = getVariable;\r\n\t}\r\n\r\n");
                        getFuction.Add("\tpublic " + exportVariableTypeName[i] + " Get" + char.ToUpper(variableName[addnum][0])
                            + variableName[addnum].Substring(1) + "()\r\n\t{\r\n\t\treturn " + variableName[addnum] + ";\r\n\t}\r\n\r\n");
                        addnum++;
                    }
                }
            }

            returnModeleCode = @"using System.Collections.Generic;
using UnityEngine;

public class " + modelScriptName + "\r\n{\r\n\t";

            for (int i = 0; i < exportvariable.Count; i++)
            {
                returnModeleCode += "private " + exportvariable[i];
            }

            for (int i = 0; i < setFuction.Count; i++)
            {
                returnModeleCode += setFuction[i];
            }

            for (int i = 0; i < getFuction.Count; i++)
            {
                returnModeleCode += getFuction[i];
            }

            returnModeleCode += "}";

            return returnModeleCode;
        }

        private string CreatePresenterCode()
        {
            string code = "using System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;\r\nusing UniRx;\r\n\r\n" +
                "public class " + presenterScriptName + " : MonoBehaviour\r\n" +
                "{\r\n" +
                "\t" + "private " + modelScriptName + " " + char.ToLower(modelScriptName[0]) + modelScriptName.Substring(1) + " = new();" +
                "\r\n\r\n" +
                "\tvoid Start()\r\n" +
                "\t{\r\n\r\n" +
                "\t}\r\n" +
                "}";
            return code;
        }
    }
}
#endif