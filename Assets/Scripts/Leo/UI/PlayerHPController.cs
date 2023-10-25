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



    public void Initialize(float maxHp)
    {
        playerHpSlider = hpSlider.GetComponent<Slider>();
        playerHpText.text = maxHp.ToString();
        currentPlayerHp = maxHp;
        Debug.Log("HP" + currentPlayerHp);
        playerHpSlider.maxValue = maxHp;
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
        playerHpSlider.value = hp;
        playerMaxHp = playerHpSlider.maxValue;

        if (currentPlayerHp > 0)
        {
            playerHpText.text = currentPlayerHp.ToString() + "/" + playerMaxHp.ToString();
        }
        if (currentPlayerHp < 0)
        {
            currentPlayerHp = 0;
            playerHpText.text = currentPlayerHp.ToString() + "/" + playerMaxHp.ToString();
        }
    }
}
