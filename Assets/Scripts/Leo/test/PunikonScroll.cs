using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PunikonScroll : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPushed = false; // マウスが押されているか押されていないか
    private Vector3 nowMousePos; // 現在のマウスのワールド座標
    private Vector3 initialPosition; // オブジェクトの初期位置
    float xLimit = 10.0f; // x軸方向の可動領域
    float yLimit = 10.0f; // y軸方向の可動領域

    void Start()
    {
        initialPosition = transform.position; // オブジェクトの初期位置を保存
    }

    void Update()
    {
        Vector3 nowMousePosi;
        Vector3 diffPos;
        Vector3 currentPos = transform.position;

        if (isPushed) {    // マウスが押下されている時、オブジェクトを動かす
            nowMousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 現在のマウスのワールド座標を取得
            diffPos = (nowMousePosi - nowMousePos) * 3; // 一つ前のマウス座標との差分を計算して変化量を取得
            diffPos.z = 0;    // z成分のみ変化させない
            transform.position += diffPos;  // オブジェクトの座標にマウスの変化量を足して新しい座標を設定
            nowMousePos = nowMousePosi; // 現在のマウスのワールド座標を更新
        }

        // 位置の制限
        // currentPos.x = Mathf.Clamp(currentPos.x, initialPosition.x - xLimit, initialPosition.x + xLimit);
        // currentPos.y = Mathf.Clamp(currentPos.y, initialPosition.y - yLimit, initialPosition.y + yLimit);
        // transform.position = currentPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPushed = true; // 押下開始　フラグを立てる
        nowMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // マウスのワールド座標を保存
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPushed = false; // 押下終了　フラグを落とす
        nowMousePos = Vector2.zero;
        transform.position = initialPosition; // オブジェクトを初期位置に戻す
    }
}
