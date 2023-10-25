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
    [Header("プレイヤーの最大HP量")] private float playerMaxHp = 100f; // プレイヤーHPの最大値
    [Header("現在のプレイヤーHP")] private float currentPlayerHp; // プレイヤーの現在のHP
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

    // プレイヤーのダメージ
    public void PlayerTakeDamage(float hp)
    {
        currentPlayerHp = hp;
        UpdatePlayerHpUI(hp);

        if (currentPlayerHp <= 0)
        {
            sceneChange.ToGameOverScene(); // 死亡処理
        }
    }

    // プレイヤーの回復
    public void PlayerTakeHealing(float healing)
    {
        currentPlayerHp += healing;
        if (currentPlayerHp >= playerMaxHp)
        {
            currentPlayerHp = playerMaxHp;
        }
        UpdatePlayerHpUI(currentPlayerHp);
    }

    // PlayerHPテキストを更新する
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
