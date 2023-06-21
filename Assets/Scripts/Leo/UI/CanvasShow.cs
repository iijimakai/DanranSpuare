using UnityEngine;

public class CanvasShow : MonoBehaviour
{
    [SerializeField, Header("難易度選択画面")] private GameObject canvasDifficulty; // 難易度Canvas
    [SerializeField, Header("メニュー画面")] private GameObject canvasMenu; // メニューCanvas
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
}
