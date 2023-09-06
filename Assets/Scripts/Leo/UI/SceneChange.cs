using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private CanvasShow canvasShow;
    [SerializeField] private float delayTime = 4f;
    //[Header("�L�����N�^�[�Љ�1")] public GameObject characterIntroductionScene1; // Character�̏Љ�Scene1�i�ҋ@���Ԃɕ\������Scene�j
    //[Header("�L�����N�^�[�Љ�2")] public GameObject characterIntroductionScene2; // Character�̏Љ�Scene2�i�ҋ@���Ԃɕ\������Scene�j
    //[Header("�L�����N�^�[�Љ�3")] public GameObject characterIntroductionScene3; // Character�̏Љ�Scene3�i�ҋ@���Ԃɕ\������Scene�j

    // TitleScene�Ŏg�p
    public void OnClickStartButton() // �^�C�g��(�X�^�[�g)�{�^��
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    private async UniTask EasyButtonSystem()
    {
        SceneManager.LoadScene("CharacterIntroductionScene1");
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
        SceneManager.LoadScene("CharacterIntroductionScene2");
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
        SceneManager.LoadScene("CharacterIntroductionScene3");
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
