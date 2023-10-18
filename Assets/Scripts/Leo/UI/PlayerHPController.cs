using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Shun_Player;

public class PlayerHPController : MonoBehaviour
{
    [SerializeField] private SceneChange sceneChange;
    public GameObject hpSlider;
    private Slider playerHpSlider;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [Header("�v���C���[�̍ő�HP��")] private float playerMaxHp = 100f; // �v���C���[HP�̍ő�l
    [Header("���݂̃v���C���[HP")] private float currentPlayerHp; // �v���C���[�̌��݂�HP
    //private TextMeshProUGUI playerHpText;
    //private Slider playerHpSlider;



    private void Awake()
    {
        //playerHpText = Instantiate(_playerHpText);
        //playerHpSlider = Instantiate(_playerHpSlider);

        playerHpSlider = hpSlider.GetComponent<Slider>();
        playerHpText.text = PlayerParameter.maxHp.ToString();
        //playerHpSlider = GetComponent<Slider>();
        currentPlayerHp = PlayerParameter.maxHp; // HP���ő�l�ŏ�����
        Debug.Log("HP"+currentPlayerHp);
        playerHpSlider.maxValue = PlayerParameter.maxHp; // HP�̍ő�l��ݒ�
        UpdatePlayerHpUI(currentPlayerHp);
    }

    // �v���C���[�̃_���[�W
    public void PlayerTakeDamage(float hp)
    {
        currentPlayerHp = hp;
        UpdatePlayerHpUI(hp);

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
        UpdatePlayerHpUI(currentPlayerHp);
    }

    // PlayerHP�e�L�X�g���X�V����
    public void UpdatePlayerHpUI(float hp)
    {
        Debug.Log(hp);
        playerHpSlider.value = hp;

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
