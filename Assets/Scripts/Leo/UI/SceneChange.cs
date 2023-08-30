using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private CanvasShow canvasShow;
    [Header("�L�����N�^�[�Љ�1")] public GameObject characterIntroductionScene1; // Character�̏Љ�Scene1�i�ҋ@���Ԃɕ\������Scene�j
    [Header("�L�����N�^�[�Љ�2")] public GameObject characterIntroductionScene2; // Character�̏Љ�Scene2�i�ҋ@���Ԃɕ\������Scene�j
    [Header("�L�����N�^�[�Љ�3")] public GameObject characterIntroductionScene3; // Character�̏Љ�Scene3�i�ҋ@���Ԃɕ\������Scene�j

    // TitleScene�Ŏg�p
    public void OnClickStartButton() // �^�C�g��(�X�^�[�g)�{�^��
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    // characterScene(��Փx�I�����)�Ŏg�p
    public async UniTask OnClickEasyButton() //�ȒP�{�^��
    {
        characterIntroductionScene1.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(8)); // �ҋ@����
        SceneManager.LoadScene("GameScene"); // �ȒPver.��GameScene�ɑJ�ڂ���\��
    }

    // characterScene(��Փx�I�����)�Ŏg�p
    public async UniTask OnClickNormalButton() // ���ʃ{�^��
    {
        characterIntroductionScene2.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(8)); // �ҋ@����
        SceneManager.LoadScene("GameScene"); // ����ver.��GameScene�ɑJ�ڂ���\��
    }

    // characterScene(��Փx�I�����)�Ŏg�p
    public async UniTask OnClickHardButton() // ����{�^��
    {
        characterIntroductionScene3.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(8)); // �ҋ@����
        SceneManager.LoadScene("GameScene"); // ���ver.��GameScene�ɑJ�ڂ���\��
    }

    // GameScene(menu���) & GameOverScene�Ŏg�p
    public void OnClickRetireButton() // ���߂�{�^��
    {
        SceneManager.LoadScene("TitleScene");
    }

    // GameScene�Ŏg�p
    //public void OnClickResultButton() // ���U���g�{�^��
    //{
    //    // boss��|��������1�{�^����UI�ŏo���ĉ�������Aclear��ʂɈړ�����
        
    //    SceneManager.LoadScene("ClearScene");
    //}

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
        canvasShow.GameOverCanvasShow();
        SceneManager.LoadScene("GameOverScene");
    }

    // PlayerHPController�Ŏg�p
    public void ToClearScene()
    {
        canvasShow.ClearCanvasShow();
        SceneManager.LoadScene("ClearScene");
    }

    // GameScene�Ŏg�p
    public void ToTitleScene()
    {
        canvasShow.GiveUpCanvasShow();
        SceneManager.LoadScene("TitleScene");
    }
}
