using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Shun_Constants;

public class CanvasShow : MonoBehaviour
{
    [SerializeField] private GameObject canvasDifficulty; // ��ՓxCanvas
    [SerializeField] private GameObject characterSelectFreamL; // �L�����N�^�[�I���t���[��L
    [SerializeField] private GameObject characterSelectFreamR; // �L�����N�^�[�I���t���[��R
    [SerializeField] private GameObject menuCanvas; // ���j���[Canvas
    [SerializeField] private GameObject youLoseCanvas; // ����Canvas
    [SerializeField] private GameObject gameOverCanvas; // �Q�[���I�[�o�[Canvas
    [SerializeField] private GameObject clearCanvas; // �Q�[���N���ACanvas
    [SerializeField] private GameObject giveUpCanvas; // �M�u�A�b�vCanvas

    private bool isPaused = false;  // GameScene���ꎞ��~�p
    [SerializeField] private float delayTime;

    [SerializeField] private SceneChange sceneChange;
    CharacterType playerType = CharacterType.P1;

    private void Awake()
    {
        if(CompareTag(TagName.Select))
        {
            OnClickCharacterButtonL();
        }
    }


    // CharacterScene�Ŏg�p
    public void OnClickConfirmButton() // �m��{�^��
    {
        canvasDifficulty.SetActive(true);
    }

    // CharacterScene�Ŏg�p
    public void OnClickCharacterButtonL() // �L�����N�^�[L�{�^��
    {
        characterSelectFreamR.SetActive(false);
        characterSelectFreamL.SetActive(true);
        playerType = CharacterType.P1;
        sceneChange.OnClickConfirmSystem(playerType);
    }

    // CharacterScene�Ŏg�p
    public void OnClickCharacterButtonR() // �L�����N�^�[R�{�^��
    {
        characterSelectFreamL.SetActive(false);
        characterSelectFreamR.SetActive(true);
        playerType = CharacterType.P2;
        sceneChange.OnClickConfirmSystem(playerType);
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
        menuCanvas.SetActive(true);
    }

    // GameScene(menu���)�Ŏg�p
    public void OnClickContinueButton() // ������{�^��
    {
        menuCanvas.SetActive(false);
        //isPaused = isPaused;
        Time.timeScale = isPaused ? 0f : 1f; // �Q�[���̎��Ԃ̐i�s���ꎞ��~�܂��͍ĊJ����
    }

    public async UniTask GameOverCanvasShow()
    {
        // �ȉ�3�s�͕K�v�ɉ����ăR�����g�A�E�g����
        youLoseCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // �ҋ@����
        youLoseCanvas.SetActive(false);
        // �����܂�

        gameOverCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // �ҋ@����
    }

    public async UniTask ClearCanvasShow()
    {
        clearCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // �ҋ@����
    }

    public void GiveUpCanvasShow()
    {
        menuCanvas.SetActive(false);
        giveUpCanvas.SetActive(true);
    }
}
