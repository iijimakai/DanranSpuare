using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPController : MonoBehaviour
{
    [SerializeField] private SceneChange sceneChange;
    public Slider playerHpSlider;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField, Header("プレイヤーの最大HP量")] private float playerMaxHp = 100f; // プレイヤーHPの最大値
    [SerializeField, Header("現在のプレイヤーHP")] private float currentPlayerHp; // プレイヤーの現在のHP

    private void Awake()
    {
        playerHpSlider = GetComponent<Slider>();
        currentPlayerHp = playerMaxHp; // HPを最大値で初期化
        playerHpSlider.maxValue = playerMaxHp; // HPの最大値を設定
        UpdatePlayerHpUI();
    }

    // プレイヤーのダメージ
    public void PlayerTakeDamage(float damage)
    {
        currentPlayerHp -= damage;
        UpdatePlayerHpUI();

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
        UpdatePlayerHpUI();
    }

    // PlayerHPテキストを更新する
    private void UpdatePlayerHpUI()
    {
        playerHpSlider.value = currentPlayerHp;
        if (currentPlayerHp > 0)
        {
            playerHpText.text = currentPlayerHp.ToString() + "/100"; // テキストにプレイヤーHPを表示
        }
        if (currentPlayerHp < 0)
        {
            currentPlayerHp = 0;
            playerHpText.text = currentPlayerHp.ToString() + "/100"; // テキストにプレイヤーHPを表示
        }
    }
}
