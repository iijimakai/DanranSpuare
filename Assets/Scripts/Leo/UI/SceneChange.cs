using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private CanvasShow canvasShow;
    [SerializeField] private float delayTime = 4f;
    //[Header("キャラクター紹介1")] public GameObject characterIntroductionScene1; // Characterの紹介Scene1（待機時間に表示するScene）
    //[Header("キャラクター紹介2")] public GameObject characterIntroductionScene2; // Characterの紹介Scene2（待機時間に表示するScene）
    //[Header("キャラクター紹介3")] public GameObject characterIntroductionScene3; // Characterの紹介Scene3（待機時間に表示するScene）

    // TitleSceneで使用
    public void OnClickStartButton() // タイトル(スタート)ボタン
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    private async UniTask EasyButtonSystem()
    {
        SceneManager.LoadScene("CharacterIntroductionScene1");
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // 待機処理
        SceneManager.LoadScene("EasyGameScene");
    }

    // CharacterScene(難易度選択画面)で使用
    public void OnClickEasyButton() // 簡単ボタン
    {
        EasyButtonSystem().Forget();
    }

    private async UniTask NormalButtonSystem()
    {
        SceneManager.LoadScene("CharacterIntroductionScene2");
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // 待機処理
        SceneManager.LoadScene("NormalGameScene");
    }

    // CharacterScene(難易度選択画面)で使用
    public void OnClickNormalButton() // 普通ボタン
    {
        NormalButtonSystem().Forget();
    }

    private async UniTask HardButtonSystem()
    {
        SceneManager.LoadScene("CharacterIntroductionScene3");
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // 待機処理
        SceneManager.LoadScene("HardGameScene");
    }

    // CharacterScene(難易度選択画面)で使用
    public void OnClickHardButton() // 難しいボタン
    {
        HardButtonSystem().Forget();
    }

    // GameScene(menu画面) & GameOverSceneで使用
    public void OnClickRetireButton() // 諦めるボタン
    {
        SceneManager.LoadScene("TitleScene");
    }

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
        canvasShow.GameOverCanvasShow().Forget();
        SceneManager.LoadScene("GameOverScene");
    }

    // PlayerHPControllerで使用
    public void ToClearScene()
    {
        canvasShow.ClearCanvasShow().Forget();
        SceneManager.LoadScene("ClearScene");
    }

    // EasyGameScene & NormalGameScene & HardGameSceneで使用
    public void ToTitleScene()
    {
        canvasShow.GiveUpCanvasShow().Forget();
        SceneManager.LoadScene("TitleScene");
    }
}
