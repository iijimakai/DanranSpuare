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



    private void Awake()
    {
        //playerHpText = Instantiate(_playerHpText);
        //playerHpSlider = Instantiate(_playerHpSlider);

        playerHpSlider = hpSlider.GetComponent<Slider>();
        playerHpText.text = PlayerParameter.maxHp.ToString();
        //playerHpSlider = GetComponent<Slider>();
        currentPlayerHp = PlayerParameter.maxHp; // HPを最大値で初期化
        Debug.Log("HP"+currentPlayerHp);
        playerHpSlider.maxValue = PlayerParameter.maxHp; // HPの最大値を設定
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
        Debug.Log(hp);
        playerHpSlider.value = hp;

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
