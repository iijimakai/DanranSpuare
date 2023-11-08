using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;
using Shun_System;
using Shun_Constants;
using System.Threading;


public class SceneChange : MonoBehaviour
{
    [SerializeField] private CanvasShow canvasShow;
    [SerializeField] private float delayTime;

    private CancellationToken cancellationToken;


    private void Awake()
    {
        cancellationToken = this.GetCancellationTokenOnDestroy();
    }

    // TitleSceneで使用
    public void OnClickStartButton() // タイトル(スタート)ボタン
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    private async UniTask EasyButtonSystem()
    {
        SceneManager.LoadScene("OperateScene1");
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
        SceneManager.LoadScene("OperateScene2");
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
        SceneManager.LoadScene("OperateScene3");
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
        //ToTitleScene();
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
    private async void ToTitleScene()
    {
        canvasShow.GiveUpCanvasShow();
        await UniTask.Delay(TimeSpan.FromSeconds(4),cancellationToken: cancellationToken); // 待機処理
        SceneManager.LoadScene("TitleScene");
    }


    /// <summary>
    /// 今回のみ使用
    /// </summary>
    //今回のみ使用するGameScene
    private async UniTask ToAlphaVersionSystem()
    {
        SceneManager.LoadScene("OperateScene1");
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // 待機処理
        SceneManager.LoadScene("AlphaVersion");
    }

    private void ToOparateScene()
    {
        SceneManager.LoadScene("OperateScene1");
    }


    //少し変更予定
    // CharacterSceneで使用
    public void OnClickConfirmSystem(CharacterType pType)
    {
        PlayerEntryPoint.characterType = pType;
    }

    public void OnClickConfirmButton() // 確定ボタン
    {
        //ToAlphaVersionSystem().Forget();
        ToOparateScene();
    }

    public void OnClickOparateSceneStartButton() // Startボタン
    {
        SceneManager.LoadScene("AlphaVersion");
    }

}
