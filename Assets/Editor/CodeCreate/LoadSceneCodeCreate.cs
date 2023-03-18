#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace CreateCode
{
    public class LoadSceneCodeCreate : EditorWindow
    {

        //書き出すスクリプトの名前
        private string sceneManagerCodeName = "SceneManagerController";
        private string loadControllerCodeName = "LoadingController";
        private string loadControllerPresenterCodeName = "Presenter";

        //書き出すフォルダパス
        private string createPath = "Assets/Scripts/";

        private bool sceneMangerControllerCheck = true;
        private bool loadControllerCreateCheck = true;
        private bool presenterCreateCheck = true;

        [MenuItem("CreateCode/SceneManagerController")]
        private static void DisplayWindow()
        {
            LoadSceneCodeCreate window = GetWindow<LoadSceneCodeCreate>();
            window.titleContent = new GUIContent("CreateMenu");
        }

        private void OnGUI()
        {
            var currentPosition = position;
            //シーンマネージャーの生成
            GUILayout.Label("シーンマネージャースクリプトを作りますか?");
            sceneMangerControllerCheck = EditorGUILayout.Toggle(sceneMangerControllerCheck);

            if (sceneMangerControllerCheck)
            {
                GUILayout.Label("Scene移行スクリプトの名前を入力してください");
                sceneManagerCodeName = EditorGUILayout.TextField(sceneManagerCodeName, GUILayout.Height(20));
            }

            //ロードシーンUIの生成
            GUILayout.Label("ロードシーンUI生成スクリプトを作りますか?");
            loadControllerCreateCheck = EditorGUILayout.Toggle(loadControllerCreateCheck);

            if (loadControllerCreateCheck)
            {
                GUILayout.Label("ロードシーンUI生成スクリプトの名前を入力してください");
                loadControllerCodeName = EditorGUILayout.TextField(loadControllerCodeName, GUILayout.Height(20));

                GUILayout.Label("この名前のPresenterも生成されます。");
                loadControllerPresenterCodeName = loadControllerCodeName + "Presenter.cs";
                GUILayout.Label(loadControllerPresenterCodeName);
                GUILayout.Label("Presenterを生成しますか?");
                presenterCreateCheck = EditorGUILayout.Toggle(presenterCreateCheck);
            }

            GUILayout.Label("以下の場所に生成されます。");
            #region
            if (sceneMangerControllerCheck)
            {
                GUILayout.Label("SceneManger : " + createPath + sceneManagerCodeName + "cs");
            }
            if (loadControllerCreateCheck)
            {
                GUILayout.Label("LoadSceneController : " + createPath + loadControllerCodeName + "cs");
                GUILayout.Label("LoadScenePresenter : " + createPath + loadControllerPresenterCodeName);
            }
            #endregion

            GUILayout.Label("UniRxとAddressableがないとエラーが出ます。");
            if (GUILayout.Button("スクリプト生成"))
            {
                if (sceneMangerControllerCheck)
                {
                    string filePath = createPath + sceneManagerCodeName + ".cs";
                    filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
                    File.WriteAllText(filePath,SceneManagerCode());
                    AssetDatabase.Refresh();
                    Debug.Log(sceneManagerCodeName + "を生成しました。");
                }

                if (loadControllerCreateCheck)
                {
                    string filePath = createPath + loadControllerCodeName + ".cs";
                    filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
                    File.WriteAllText(filePath, LoadControllerCode());
                    AssetDatabase.Refresh();
                    Debug.Log(loadControllerCodeName + "を生成しました。");

                    if (presenterCreateCheck)
                    {
                        filePath = createPath + loadControllerPresenterCodeName + ".cs";
                        filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
                        File.WriteAllText(filePath, LoadControllerPresenterCode());
                        AssetDatabase.Refresh();
                        Debug.Log(loadControllerPresenterCodeName + "を生成しました。");
                    }
                }
            }

            position = currentPosition;
        }

        private string SceneManagerCode()
        {
            string code = @"using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UniRx;
using System;

namespace SceneaManger
{
    public class " + sceneManagerCodeName + @" : MonoBehaviour
    {
        private Subject<AsyncOperation> loadScene = new Subject<AsyncOperation>();

        public void LoadScene(string scenePath)
        {
            loadScene.OnNext(SceneManager.LoadSceneAsync(scenePath));
        }

        public IObservable<AsyncOperation> GetLoadScene()
        {
            return loadScene;
        }
    }
}";
            return code;
        }

        private string LoadControllerCode()
        {
            string code = @"using UnityEngine;
using UnityEngine.UI;

namespace SceneaManger
{
    public class " + loadControllerCodeName + @" : MonoBehaviour
    {

        [SerializeField, Header(""ロード画面で表示する画面"")]
        private GameObject loadingUI;

        [SerializeField, Header(""シーン移動の進行度"")]
        private Slider loadSlider;

        public void LoadData(AsyncOperation load)
        {
            loadingUI.SetActive(true);
            while (!load.isDone)
            {
                var progressVal = Mathf.Clamp01(load.progress / 0.9f);
                loadSlider.value = progressVal;
                if (loadSlider.value >= 0.9f)
                {
                    break;
                }
            }
        }
    }
}";
            return code;
        }

        private string LoadControllerPresenterCode()
        {
            string code = @"using UnityEngine;
using UniRx;

namespace SceneaManger
{
    public class LoadingControllerPresenter : MonoBehaviour
    {
        [SerializeField, Header(""以下参照スクリプト----------------------"")]
        private LoadingController loading;

        [SerializeField]
        private SceneManagerController sceneManager;

        void Start()
        {
            sceneManager.GetLoadScene()
                .Subscribe(scene => loading.LoadData(scene))
                .AddTo(this);
        }
    }
}";
            return code;
        }
    }
}
#endif