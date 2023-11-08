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

    // TitleScene�Ŏg�p
    public void OnClickStartButton() // �^�C�g��(�X�^�[�g)�{�^��
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    private async UniTask EasyButtonSystem()
    {
        SceneManager.LoadScene("OperateScene1");
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // �ҋ@����
        SceneManager.LoadScene("EasyGameScene");
    }

    // CharacterScene(��Փx�I�����)�Ŏg�p
    public void OnClickEasyButton() // �ȒP�{�^��
    {
        EasyButtonSystem().Forget();
    }

    private async UniTask NormalButtonSystem()
    {
        SceneManager.LoadScene("OperateScene2");
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // �ҋ@����
        SceneManager.LoadScene("NormalGameScene");
    }

    // CharacterScene(��Փx�I�����)�Ŏg�p
    public void OnClickNormalButton() // ���ʃ{�^��
    {
        NormalButtonSystem().Forget();
    }

    private async UniTask HardButtonSystem()
    {
        SceneManager.LoadScene("OperateScene3");
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // �ҋ@����
        SceneManager.LoadScene("HardGameScene");
    }

    // CharacterScene(��Փx�I�����)�Ŏg�p
    public void OnClickHardButton() // ����{�^��
    {
        HardButtonSystem().Forget();
    }

    // GameScene(menu���) & GameOverScene�Ŏg�p
    public void OnClickRetireButton() // ���߂�{�^��
    {
        //ToTitleScene();
        SceneManager.LoadScene("TitleScene");
    }

    // ClearScene�Ŏg�p
    public void OnClickEndButton() // �I���{�^��
    {
        SceneManager.LoadScene("TitleScene");
    }

    // GameOverScene�Ŏg�p
    public void OnClickRetryButton() // ������x�{�^��
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    // PlayerHPController�Ŏg�p
    public void ToGameOverScene()
    {
        canvasShow.GameOverCanvasShow().Forget();
        SceneManager.LoadScene("GameOverScene");
    }

    // PlayerHPController�Ŏg�p
    public void ToClearScene()
    {
        canvasShow.ClearCanvasShow().Forget();
        SceneManager.LoadScene("ClearScene");
    }

    // EasyGameScene & NormalGameScene & HardGameScene�Ŏg�p
    private async void ToTitleScene()
    {
        canvasShow.GiveUpCanvasShow();
        await UniTask.Delay(TimeSpan.FromSeconds(4),cancellationToken: cancellationToken); // �ҋ@����
        SceneManager.LoadScene("TitleScene");
    }


    /// <summary>
    /// ����̂ݎg�p
    /// </summary>
    //����̂ݎg�p����GameScene
    private async UniTask ToAlphaVersionSystem()
    {
        SceneManager.LoadScene("OperateScene1");
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // �ҋ@����
        SceneManager.LoadScene("AlphaVersion");
    }

    private void ToOparateScene()
    {
        SceneManager.LoadScene("OperateScene1");
    }


    //�����ύX�\��
    // CharacterScene�Ŏg�p
    public void OnClickConfirmSystem(CharacterType pType)
    {
        PlayerEntryPoint.characterType = pType;
    }

    public void OnClickConfirmButton() // �m��{�^��
    {
        //ToAlphaVersionSystem().Forget();
        ToOparateScene();
    }

    public void OnClickOparateSceneStartButton() // Start�{�^��
    {
        SceneManager.LoadScene("AlphaVersion");
    }

}
