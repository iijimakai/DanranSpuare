using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections;
using pool;

public class BossAttack : MonoBehaviour
{
    private BossDataLoader bossDataLoader;
    [SerializeField] private BossBulletPool bulletPool;
    private int maxActiveBullets;
    private float bulletMoveSpeed;
    private int activeBulletsCount = 0;
    private GameObject player;
    private BossAttackRange bossAttackRange;
    private BossMovement bossMovement;
    private bool isOnCooldown = false;
    private float warningToBreathDelay;
    private float postBreathInterval;
    private float bulletSpawnInterval;
    private float bulletLeftSideWidth;
    private float bulletRightSideWidth;
    private async UniTask Start()
    {
        bossDataLoader = GetComponent<BossDataLoader>();
        await bossDataLoader.LoadBossData();

        await WaitForPlayerSpawn();

        bulletPool = BossBulletPool.Instance;
        player = GameObject.FindGameObjectWithTag(TagName.Player);
        bossAttackRange = GetComponentInParent<BossAttack>().GetComponentInChildren<BossAttackRange>();
        bossMovement = GetComponent<BossMovement>();
        warningToBreathDelay = bossDataLoader.bossData.warningToBreathDelay;
        postBreathInterval = bossDataLoader.bossData.postBreathInterval;
        bulletSpawnInterval = bossDataLoader.bossData.bulletSpawnInterval;
        bulletLeftSideWidth = bossDataLoader.bossData.bulletLeftSideWidth;
        bulletRightSideWidth = bossDataLoader.bossData.bulletRightSideWidth;
    }
    private async UniTask WaitForPlayerSpawn()
    {
        while (GameObject.FindGameObjectWithTag(TagName.Player) == null)
        {
            await UniTask.Delay(500); // 0.5秒ごとに再試行
        }
    }
    public void InitializeAttackData(float bulletSpeed, int maxBullets)
    {
        this.bulletMoveSpeed = bulletSpeed;
        this.maxActiveBullets = maxBullets;
    }

    public void PrepareAttack(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = new Color(255, 0, 0);
    }

    public void PrepareAttackCancel(SpriteRenderer spriteRenderer, Color originalColor)
    {
        spriteRenderer.color = originalColor;
    }
    // BulletPoolからオブジェクトを取得し、nullでないことを確認
    private GameObject GetBullet()
    {
        var bullet = bulletPool.GetObject();
        if (bullet == null) {
            Debug.LogWarning("Bullet object is destroyed or pool is empty.");
        }
        return bullet;
    }
    public async UniTask BreathAttack(Transform bossTransform)
    {
        // このキャンセルトークンはこの MonoBehaviour が破棄されたときにキャンセルされる
        var cancellationToken = this.GetCancellationTokenOnDestroy();
        if(isOnCooldown) return;

        isOnCooldown = true;
        bossMovement.isBreathing = true; // BreathAttack開始
        // 警告アラートを表示
        await bossAttackRange.ShowWarningAlert();

        // 警告アラートが表示されてから何秒後にブレス攻撃を開始するか
        await UniTask.Delay(TimeSpan.FromSeconds(warningToBreathDelay));

        // 指定時間、連続的に弾を発射する
        float breathDuration = bossDataLoader.bossData.breathDuration; // 例: ブレスを2秒間続ける
        float startTime = Time.time;

        while(Time.time - startTime < breathDuration && !cancellationToken.IsCancellationRequested)
        {
            if (activeBulletsCount < maxActiveBullets)
            {
                var bullet = GetBullet();
                if(bullet == null) return;
                // 三つの弾を一度に発射
                Vector3 offsetLeft = new Vector3(bulletLeftSideWidth, 0, 0);
                Vector3 offsetRight = new Vector3(bulletRightSideWidth, 0, 0);

                // 中央の弾
                _ = MoveBullet(bulletPool.GetObject(), bossTransform.position, bossTransform);

                // 左の弾
                _ = MoveBullet(bulletPool.GetObject(), bossTransform.position + offsetLeft, bossTransform);

                // 右の弾
                _ = MoveBullet(bulletPool.GetObject(), bossTransform.position + offsetRight, bossTransform);
            }

            await UniTask.Delay(TimeSpan.FromMilliseconds(bulletSpawnInterval));
        }
        // ブレス後のインターバル
        await UniTask.Delay(TimeSpan.FromMilliseconds(postBreathInterval));
        isOnCooldown = false;
        bossMovement.isBreathing = false; // BreathAttack終了
    }
    private async UniTask MoveBullet(GameObject bullet, Vector3 startPosition, Transform bossTransform)
    {
        // bullet または player が破棄されているかを確認
        if (bullet == null || player == null) return;
        bullet.transform.position = startPosition;
        if (bullet.GetComponent<BossBulletDamage>() == null) // 例外処理
        {
            bullet.AddComponent<BossBulletDamage>();
        }
        Vector3 direction = (player.transform.position - bossTransform.position).normalized;
        // ここでキャンセルトークンを取得します
        var cancellationToken = this.GetCancellationTokenOnDestroy();

        try
        {
            for (float movedDistance = 0; movedDistance < 30f; movedDistance += bulletMoveSpeed * Time.deltaTime)
            {
                // オブジェクトが破棄されていないか&キャンセルされていないかを確認
                if (bullet == null || player == null || cancellationToken.IsCancellationRequested) break;

                bullet.transform.position += direction * bulletMoveSpeed * Time.deltaTime;
                // 次のフレームまで待機
                await UniTask.NextFrame(cancellationToken);
            }
        }
        finally
        {
            // 安全なクリーンアップ
            if (bullet != null)
            {
                bullet.SetActive(false);
            }
            activeBulletsCount--;
        }
    }
}
