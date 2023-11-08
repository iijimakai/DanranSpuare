using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Shun_Constants;

public class CanvasShow : MonoBehaviour
{
    [SerializeField] private GameObject canvasDifficulty; // 難易度Canvas
    [SerializeField] private GameObject characterSelectFreamL; // キャラクター選択フレームL
    [SerializeField] private GameObject characterSelectFreamR; // キャラクター選択フレームR
    [SerializeField] private GameObject menuCanvas; // メニューCanvas
    [SerializeField] private GameObject youLoseCanvas; // 負けCanvas
    [SerializeField] private GameObject gameOverCanvas; // ゲームオーバーCanvas
    [SerializeField] private GameObject clearCanvas; // ゲームクリアCanvas
    [SerializeField] private GameObject giveUpCanvas; // ギブアップCanvas

    private bool isPaused = false;  // GameSceneを一時停止用
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


    // CharacterSceneで使用
    public void OnClickConfirmButton() // 確定ボタン
    {
        canvasDifficulty.SetActive(true);
    }

    // CharacterSceneで使用
    public void OnClickCharacterButtonL() // キャラクターLボタン
    {
        characterSelectFreamR.SetActive(false);
        characterSelectFreamL.SetActive(true);
        playerType = CharacterType.P1;
        sceneChange.OnClickConfirmSystem(playerType);
    }

    // CharacterSceneで使用
    public void OnClickCharacterButtonR() // キャラクターRボタン
    {
        characterSelectFreamL.SetActive(false);
        characterSelectFreamR.SetActive(true);
        playerType = CharacterType.P2;
        sceneChange.OnClickConfirmSystem(playerType);
    }

    // CharacterScene(難易度選択画面)で使用
    public void OnClickReturnButton() // ←戻るボタン
    {
        canvasDifficulty.SetActive(false);
    }

    // GameSceneで使用
    public void OnClickMenuButton() // メニューボタン
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f; // ゲームの時間の進行を一時停止または再開する
        menuCanvas.SetActive(true);
    }

    // GameScene(menu画面)で使用
    public void OnClickContinueButton() // 続けるボタン
    {
        menuCanvas.SetActive(false);
        //isPaused = isPaused;
        Time.timeScale = isPaused ? 0f : 1f; // ゲームの時間の進行を一時停止または再開する
    }

    public async UniTask GameOverCanvasShow()
    {
        // 以下3行は必要に応じてコメントアウトして
        youLoseCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // 待機処理
        youLoseCanvas.SetActive(false);
        // ここまで

        gameOverCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // 待機処理
    }

    public async UniTask ClearCanvasShow()
    {
        clearCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime)); // 待機処理
    }

    public void GiveUpCanvasShow()
    {
        menuCanvas.SetActive(false);
        giveUpCanvas.SetActive(true);
    }
}
