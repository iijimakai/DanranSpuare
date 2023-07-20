using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public Button LeftButton;
    public Button RightButton;
    public GameObject[] Characters; // キャラクター配列
    [SerializeField,Header("ローテーションスピード")]private float Speed = 1f; // ローテーションスピード
    [SerializeField,Header("ローテーション中のサイズ変更速度")] private float ScaleSpeed = 1f; // スケール変更のスピード
    [SerializeField,Header("スケールの初期値")] private float NormalScale = 1f; // 通常のスケール
    [SerializeField,Header("選択画面のキャラクタースケール")] private float LargerScale = 1.5f; // 選択画面のスケール
    private Vector3[] targetPositions; // 各キャラクターの目標位置
    private int[] targetIndex; // 各キャラクターの目標位置のインデックス
    private float[] targetScales; // 各キャラクターの目標スケール

    void Start()
    {
        // ボタンに関数を割り当てる
        LeftButton.onClick.AddListener(() => ShiftPositions(1));
        RightButton.onClick.AddListener(() => ShiftPositions(-1));

        // 各キャラクターの初期位置と目標位置のインデックスを設定
        targetPositions = new Vector3[Characters.Length];
        targetIndex = new int[Characters.Length];
        targetScales = new float[Characters.Length];
        for (int i = 0; i < Characters.Length; i++)
        {
            targetPositions[i] = Characters[i].transform.position;
            targetIndex[i] = i;
            Characters[i].transform.localScale = Vector3.one * NormalScale;
            targetScales[i] = NormalScale;
        }
    }
    // TODO:適切な方法でこの関数を呼び出す
    void StartGame()
    {
        // 下側の頂点にあるキャラクターを特定
        for (int i = 0; i < Characters.Length; i++)
        {
            // 選択されているオブジェクトの場合
            if (targetIndex[i] == 0)
            {
                // タグに基づいてシーンをロード
                switch (Characters[i].tag)
                {
                    // 三体のキャラクターのそれぞれにcaseの中のTagをアタッチして
                    case "Character1":
                        SceneManager.LoadScene("Character1Scene"); // 適切なシーン名に変更して
                        break;
                    case "Character2":
                        SceneManager.LoadScene("Character2Scene");// 適切なシーン名に変更して
                        break;
                    case "Character3":
                        SceneManager.LoadScene("Character3Scene");// 適切なシーン名に変更して
                        break;
                    default:
                        Debug.LogError("タグが設定されていない");
                        break;
                }
                break;
            }
        }
    }
    void Update()
    {
        // 各キャラクターを目標位置に向けて移動
        for (int i = 0; i < Characters.Length; i++)
        {
            Characters[i].transform.position = Vector3.MoveTowards(
                Characters[i].transform.position, targetPositions[i], Speed * Time.deltaTime);

            Characters[i].transform.localScale = Vector3.Lerp(
                Characters[i].transform.localScale, Vector3.one * targetScales[i], ScaleSpeed * Time.deltaTime);
        }
    }

    void ShiftPositions(int direction)
    {
        // 目標位置とそのインデックスをシフト
        if (direction > 0)
        {
            Vector3 temp = targetPositions[0];
            targetPositions[0] = targetPositions[2];
            targetPositions[2] = targetPositions[1];
            targetPositions[1] = temp;

            int tempIndex = targetIndex[0];
            targetIndex[0] = targetIndex[2];
            targetIndex[2] = targetIndex[1];
            targetIndex[1] = tempIndex;

            float tempScale = targetScales[0];
            targetScales[0] = targetScales[2];
            targetScales[2] = targetScales[1];
            targetScales[1] = tempScale;
        }
        else
        {
            Vector3 temp = targetPositions[0];
            targetPositions[0] = targetPositions[1];
            targetPositions[1] = targetPositions[2];
            targetPositions[2] = temp;

            int tempIndex = targetIndex[0];
            targetIndex[0] = targetIndex[1];
            targetIndex[1] = targetIndex[2];
            targetIndex[2] = tempIndex;

            float tempScale = targetScales[0];
            targetScales[0] = targetScales[1];
            targetScales[1] = targetScales[2];
            targetScales[2] = tempScale;
        }

        // 新たなスケールを設定
        for (int i = 0; i < Characters.Length; i++)
        {
            if (targetIndex[i] == 0) // 下側の頂点に到着する場合
            {
                targetScales[i] = LargerScale;
            }
            else
            {
                targetScales[i] = NormalScale;
            }
        }
    }
}
