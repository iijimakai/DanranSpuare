using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections;

public class BossAttackRange : MonoBehaviour
{
    private BossDataLoader bossDataLoader;
    public CompositeDisposable disposables = new CompositeDisposable();
    private GameObject player;
    private GameObject boss;
    private BossScript bossScript;
    public float rotationSpeed = 5.0f;
    public float lerpFactor = 0f;  // ボスとプレイヤーの間での位置を調整（0がボス、1がプレイヤー）
    public float someFixedDistance = 0;
    private bool isOnCooldown = false;
    private float alertCooldownDuration = 0f;
    private float alertDisplayDuration;
    [SerializeField] private GameObject warningAlert;

    private async UniTask Start()
    {
        warningAlert.SetActive(false);
        bossDataLoader = GetComponent<BossDataLoader>();
        await bossDataLoader.LoadBossData();

        await WaitForPlayerSpawn();

        boss = transform.parent.gameObject;
        player = GameObject.FindGameObjectWithTag(TagName.Player);
        bossScript = boss.GetComponent<BossScript>();
        LookAtPlayer();
        alertDisplayDuration = bossDataLoader.bossData.alertDisplayDuration;
    }
    private async UniTask WaitForPlayerSpawn()
    {
        while (GameObject.FindGameObjectWithTag(TagName.Player) == null)
        {
            await UniTask.Delay(500); // 0.5秒ごとに再試行
        }
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
    public async UniTask ShowWarningAlert()
    {
        // 警告アラートをプレイヤーの方向を向くように設定
        Vector3 directionToPlayer = (player.transform.position - boss.transform.position).normalized;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        warningAlert.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        Debug.Log("Angle to Player: " + angle);

        // プレイヤーの近くの位置を計算
        float distanceFromPlayer = bossDataLoader.bossData.alertDistanceFromPlayer; // プレイヤーからの距離を調整
        Vector3 alertPosition = player.transform.position - (directionToPlayer * distanceFromPlayer);
        warningAlert.transform.position = alertPosition;
        Debug.DrawLine(boss.transform.position, boss.transform.position + directionToPlayer * 10, Color.red, 3f);

        // 警告アラートを表示
        warningAlert.SetActive(true);
        Observable.Timer(TimeSpan.FromSeconds(1))
            .Subscribe(_ => warningAlert.SetActive(false))
            .AddTo(disposables);
    }
    private void OnTriggerStay2D(Collider2D col)
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
