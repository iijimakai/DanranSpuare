using UnityEngine;

public class CanvasShow : MonoBehaviour
{
    [SerializeField, Header("��Փx�I�����")] private GameObject canvasDifficulty; // ��ՓxCanvas
    [SerializeField, Header("���j���[���")] private GameObject canvasMenu; // ���j���[Canvas
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
}
