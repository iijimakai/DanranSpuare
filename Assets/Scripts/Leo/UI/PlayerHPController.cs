using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPController : MonoBehaviour
{
    [SerializeField] private SceneChange sceneChange;
    public Slider playerHpSlider;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField, Header("�v���C���[�̍ő�HP��")] private float playerMaxHp = 100f; // �v���C���[HP�̍ő�l
    [SerializeField, Header("���݂̃v���C���[HP")] private float currentPlayerHp; // �v���C���[�̌��݂�HP

    private void Awake()
    {
        playerHpSlider = GetComponent<Slider>();
        currentPlayerHp = playerMaxHp; // HP���ő�l�ŏ�����
        playerHpSlider.maxValue = playerMaxHp; // HP�̍ő�l��ݒ�
        UpdatePlayerHpUI();
    }

    // �v���C���[�̃_���[�W
    public void PlayerTakeDamage(float damage)
    {
        currentPlayerHp -= damage;
        UpdatePlayerHpUI();

        if (currentPlayerHp <= 0)
        {
            sceneChange.ToGameOverScene(); // ���S����
        }
    }

    // �v���C���[�̉�
    public void PlayerTakeHealing(float healing)
    {
        currentPlayerHp += healing;
        if (currentPlayerHp >= playerMaxHp)
        {
            currentPlayerHp = playerMaxHp;
        }
        UpdatePlayerHpUI();
    }

    // PlayerHP�e�L�X�g���X�V����
    private void UpdatePlayerHpUI()
    {
        playerHpSlider.value = currentPlayerHp;
        if (currentPlayerHp > 0)
        {
            playerHpText.text = currentPlayerHp.ToString() + "/100"; // �e�L�X�g�Ƀv���C���[HP��\��
        }
        if (currentPlayerHp < 0)
        {
            currentPlayerHp = 0;
            playerHpText.text = currentPlayerHp.ToString() + "/100"; // �e�L�X�g�Ƀv���C���[HP��\��
        }
    }
}
