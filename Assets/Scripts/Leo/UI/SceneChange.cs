using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private CanvasShow canvasShow;
    [SerializeField] private float delayTime = 8f;
    
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
    public void ToTitleScene()
    {
        canvasShow.GiveUpCanvasShow().Forget();
        SceneManager.LoadScene("TitleScene");
    }
}
