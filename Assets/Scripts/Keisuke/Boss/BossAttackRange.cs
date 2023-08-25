using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

public class BossAttackRange : MonoBehaviour
{
    public CompositeDisposable disposables = new CompositeDisposable();
    private GameObject player;
    private GameObject boss;
    private BossScript bossScript;
    public float rotationSpeed = 5.0f;
    public float lerpFactor = 0f;  // ボスとプレイヤーの間での位置を調整（0がボス、1がプレイヤー）
    public float someFixedDistance = 0;
    void Start()
    {
        boss = transform.parent.gameObject;
        player = GameObject.FindGameObjectWithTag(TagName.Player);
        bossScript = boss.GetComponent<BossScript>();
        LookAtPlayer();
    }
    private void LookAtPlayer()
    {
        this.UpdateAsObservable().Subscribe(_ =>
        {
            if (player != null)
            {
                // プレイヤーに向かう方向を計算
                Vector3 directionToPlayer = (player.transform.position - boss.transform.position).normalized;

                // ボスから一定の距離だけ離れた点を計算
                Vector3 offsetPosition = boss.transform.position + (directionToPlayer * someFixedDistance);

                // トライアングルをその点に移動
                transform.position = offsetPosition;

                // Z軸周りで回転させる
                float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 180);
            }
        }).AddTo(disposables);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TagName.Player))
        {
            bossScript.SetPlayerInRange(true);
            bossScript.SetTargetPosition(col.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TagName.Player))
        {
            bossScript.SetPlayerInRange(false);
        }
    }
}
