using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // TitleSceneで使用
    public void OnClickStartButton() // タイトル(スタート)ボタン
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    // 難易度によってGameSceneが変わるのか、1つのGameScene上で出てくる敵の種類が変わることで難易度を変更しているのかを確認
    // characterScene(難易度選択画面)で使用
    public void OnClickEasyButton() //簡単ボタン
    {
        SceneManager.LoadScene("GameScene");
    }

    // characterScene(難易度選択画面)で使用
    public void OnClickNormalButton() // 普通ボタン
    {
        SceneManager.LoadScene("GameScene");
    }

    // characterScene(難易度選択画面)で使用
    public void OnClickHardButton() // 難しいボタン
    {
        SceneManager.LoadScene("GameScene");
    }

    // GameScene(menu画面) & GameOverSceneで使用
    public void OnClickRetireButton() // 諦めるボタン
    {
        SceneManager.LoadScene("TitleScene");
    }

    // GameSceneで使用
    public void OnClickResultButton() // リザルトボタン
    {
        // bossを倒した時に1つボタンをUIで出して押したら、clear画面に移動する
        SceneManager.LoadScene("ClearScene");
    }

    // ClearSceneで使用
    public void OnClickEndButton() // 終了ボタン
    {
        SceneManager.LoadScene("TitleScene");
    }

    // GameOverSceneで使用
    public void OnClickRetryButton() // もう一度ボタン
    {
        SceneManager.LoadScene("GameScene");
    }

    //LRボタンは別スクリプトで書く
}
