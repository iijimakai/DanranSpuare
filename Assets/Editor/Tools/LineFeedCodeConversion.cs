using UnityEngine;
using UnityEditor;
using System.IO;

public enum lineFeelCode
{
    CR = 0,
    LF = 1,
    CRLF = 2
};

namespace Mytool
{
    public class LineFeedCodeConversion : EditorWindow
    {
        private string filepath = "";

        private lineFeelCode lineFeel_Code;

        [MenuItem("MyTools/LineFeedCodeConversion")]
        private static void DisplayWindow()
        {
            LineFeedCodeConversion window = GetWindow<LineFeedCodeConversion>();
            window.titleContent = new GUIContent("改行コード変換");
        }

        private void OnGUI()
        {
            GUILayout.Label("ファイルパス");
            GUILayout.Label(filepath, GUILayout.Height(20));

            if (GUILayout.Button("パスの指定"))
            {
                filepath = EditorUtility.OpenFilePanel("Select Asset", Application.dataPath, "csv");
                if (string.IsNullOrEmpty(filepath))
                {
                    return;
                }
            }

            GUILayout.Label("統一する改行コードを選択してください");
            lineFeel_Code = (lineFeelCode)EditorGUILayout.EnumPopup("改行コード", lineFeel_Code);

            if (filepath != "")
            {
                if (GUILayout.Button("変換"))
                {
                    Conversion();
                    Debug.Log("変換が完了しました。");
                }
            }
            else
            {
                GUILayout.Label("ファイルパスを設定してください");
            }
        }

        private void Conversion()
        {
            StreamReader sr = new StreamReader(filepath);
            string writing = "";
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                line = line.Replace("\n", "").Replace("\r\r", "");
                switch (lineFeel_Code)
                {
                    case lineFeelCode.CR:
                        line += "\r";
                        break;
                    case lineFeelCode.LF:
                        line += "\n";
                        break;
                    case lineFeelCode.CRLF:
                        line += "\r\n";
                        break;
                }
                writing += line;
            }
            sr.Close();

            StreamWriter sw = new StreamWriter(filepath);
            sw.Write(writing);
            sw.Close();
        }
    }
}