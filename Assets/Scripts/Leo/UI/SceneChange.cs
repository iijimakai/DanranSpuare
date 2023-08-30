using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private CanvasShow canvasShow;
    [Header("キャラクター紹介1")] public GameObject characterIntroductionScene1; // Characterの紹介Scene1（待機時間に表示するScene）
    [Header("キャラクター紹介2")] public GameObject characterIntroductionScene2; // Characterの紹介Scene2（待機時間に表示するScene）
    [Header("キャラクター紹介3")] public GameObject characterIntroductionScene3; // Characterの紹介Scene3（待機時間に表示するScene）

    // TitleSceneで使用
    public void OnClickStartButton() // タイトル(スタート)ボタン
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    // characterScene(難易度選択画面)で使用
    public async UniTask OnClickEasyButton() //簡単ボタン
    {
        characterIntroductionScene1.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(8)); // 待機処理
        SceneManager.LoadScene("GameScene"); // 簡単ver.のGameSceneに遷移する予定
    }

    // characterScene(難易度選択画面)で使用
    public async UniTask OnClickNormalButton() // 普通ボタン
    {
        characterIntroductionScene2.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(8)); // 待機処理
        SceneManager.LoadScene("GameScene"); // 普通ver.のGameSceneに遷移する予定
    }

    // characterScene(難易度選択画面)で使用
    public async UniTask OnClickHardButton() // 難しいボタン
    {
        characterIntroductionScene3.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(8)); // 待機処理
        SceneManager.LoadScene("GameScene"); // 難しいver.のGameSceneに遷移する予定
    }

    // GameScene(menu画面) & GameOverSceneで使用
    public void OnClickRetireButton() // 諦めるボタン
    {
        SceneManager.LoadScene("TitleScene");
    }

    // GameSceneで使用
    //public void OnClickResultButton() // リザルトボタン
    //{
    //    // bossを倒した時に1つボタンをUIで出して押したら、clear画面に移動する
        
    //    SceneManager.LoadScene("ClearScene");
    //}

    // ClearSceneで使用
    public void OnClickEndButton() // 終了ボタン
    {
        SceneManager.LoadScene("TitleScene");
    }

    // GameOverSceneで使用
    public void OnClickRetryButton() // もう一度ボタン
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    // PlayerHPControllerで使用
    public void ToGameOverScene()
    {
        canvasShow.GameOverCanvasShow();
        SceneManager.LoadScene("GameOverScene");
    }

    // PlayerHPControllerで使用
    public void ToClearScene()
    {
        canvasShow.ClearCanvasShow();
        SceneManager.LoadScene("ClearScene");
    }

    // GameSceneで使用
    public void ToTitleScene()
    {
        canvasShow.GiveUpCanvasShow();
        SceneManager.LoadScene("TitleScene");
    }
}
