using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class CanvasShow : MonoBehaviour
{
    [SerializeField] private GameObject canvasDifficulty; // 難易度Canvas
    [SerializeField] private GameObject canvasMenu; // メニューCanvas
    [SerializeField] private GameObject youLoseCanvas; // 負けCanvas
    [SerializeField] private GameObject gameOverCanvas; // ゲームオーバーCanvas
    [SerializeField] private GameObject clearCanvas; // ゲームクリアCanvas
    [SerializeField] private GameObject giveUpCanvas; // ギブアップCanvas
    private bool isPaused = false; // GameSceneを一時停止するための変数


    // CharacterSceneで使用
    public void OnClickConfirmButton() // 確定ボタン
    {
        canvasDifficulty.SetActive(true);
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
        canvasMenu.SetActive(true);
    }

    // GameScene(menu画面)で使用
    public void OnClickContinueButton() // 続けるボタン
    {
        canvasMenu.SetActive(false);
        //isPaused = isPaused;
        Time.timeScale = isPaused ? 0f : 1f; // ゲームの時間の進行を一時停止または再開する
    }

    public async UniTask GameOverCanvasShow()
    {
        // 以下3行は必要に応じてコメントアウトして
        youLoseCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(3)); // 待機処理
        youLoseCanvas.SetActive(false);
        // ここまで

        gameOverCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(4)); // 待機処理
    }

    public async UniTask ClearCanvasShow()
    {
        clearCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(4)); // 待機処理
    }

    public async UniTask GiveUpCanvasShow()
    {
        giveUpCanvas.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(4)); // 待機処理
    }
}
