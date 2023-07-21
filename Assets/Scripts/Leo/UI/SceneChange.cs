using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // TitleScene�Ŏg�p
    public void OnClickStartButton() // �^�C�g��(�X�^�[�g)�{�^��
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    // ��Փx�ɂ����GameScene���ς��̂��A1��GameScene��ŏo�Ă���G�̎�ނ��ς�邱�Ƃœ�Փx��ύX���Ă���̂����m�F
    // characterScene(��Փx�I�����)�Ŏg�p
    public void OnClickEasyButton() //�ȒP�{�^��
    {
        SceneManager.LoadScene("GameScene");
    }

    // characterScene(��Փx�I�����)�Ŏg�p
    public void OnClickNormalButton() // ���ʃ{�^��
    {
        SceneManager.LoadScene("GameScene");
    }

    // characterScene(��Փx�I�����)�Ŏg�p
    public void OnClickHardButton() // ����{�^��
    {
        SceneManager.LoadScene("GameScene");
    }

    // GameScene(menu���) & GameOverScene�Ŏg�p
    public void OnClickRetireButton() // ���߂�{�^��
    {
        SceneManager.LoadScene("TitleScene");
    }

    // GameScene�Ŏg�p
    public void OnClickResultButton() // ���U���g�{�^��
    {
        // boss��|��������1�{�^����UI�ŏo���ĉ�������Aclear��ʂɈړ�����
        SceneManager.LoadScene("ClearScene");
    }

    // ClearScene�Ŏg�p
    public void OnClickEndButton() // �I���{�^��
    {
        SceneManager.LoadScene("TitleScene");
    }

    // GameOverScene�Ŏg�p
    public void OnClickRetryButton() // ������x�{�^��
    {
        SceneManager.LoadScene("GameScene");
    }

    //LR�{�^���͕ʃX�N���v�g�ŏ���
}
