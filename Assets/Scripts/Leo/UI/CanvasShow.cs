using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class CanvasShow : MonoBehaviour
{
    [SerializeField] private GameObject canvasDifficulty; // ��ՓxCanvas
    [SerializeField] private GameObject canvasMenu; // ���j���[Canvas
    [SerializeField] private GameObject youLoseCanvas; // ����Canvas
    [SerializeField] private GameObject gameOverCanvas; // �Q�[���I�[�o�[Canvas
    [SerializeField] private GameObject clearCanvas; // �Q�[���N���ACanvas
    [SerializeField] private GameObject giveUpCanvas; // �M�u�A�b�vCanvas
    private bool isPaused = false; // GameScene���ꎞ��~���邽�߂̕ϐ�


    // CharacterScene�Ŏg�p
    public void OnClickConfirmButton() // �m��{�^��
    {
        canvasDifficulty.SetActive(true);
    }

    // CharacterScene(��Փx�I�����)�Ŏg�p
    public void OnClickReturnButton() // ���߂�{�^��
    {
        canvasDifficulty.SetActive(false);
    }

    // GameScene�Ŏg�p
    public void OnClickMenuButton() // ���j���[�{�^��
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f; // �Q�[���̎��Ԃ̐i�s���ꎞ��~�܂��͍ĊJ����
        canvasMenu.SetActive(true);
    }

    // GameScene(menu���)�Ŏg�p
    public void OnClickContinueButton() // ������{�^��
    {
        canvasMenu.SetActive(false);
        //isPaused = isPaused;
        Time.timeScale = isPaused ? 0f : 1f; // �Q�[���̎��Ԃ̐i�s���ꎞ��~�܂��͍ĊJ����
    }

    public async UniTask GameOverCanvasShow()
    {
        // �ȉ�3�s�͕K�v�ɉ����ăR�����g�A�E�g����
        youLoseCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(3)); // �ҋ@����
        youLoseCanvas.SetActive(false);
        // �����܂�

        gameOverCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(4)); // �ҋ@����
    }

    public async UniTask ClearCanvasShow()
    {
        clearCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(4)); // �ҋ@����
    }

    public async UniTask GiveUpCanvasShow()
    {
        giveUpCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(4)); // �ҋ@����
    }
}
