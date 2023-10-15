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
    private bool isOnCooldown = false;
    private float warningToBreathDelay;
    private float postBreathInterval;
    private float bulletSpawnInterval;
    private async UniTask Start()
    {
        bossDataLoader = GetComponent<BossDataLoader>();
        await bossDataLoader.LoadBossData();

        await WaitForPlayerSpawn();

        bulletPool = BossBulletPool.Instance;
        player = GameObject.FindGameObjectWithTag(TagName.Player);
        bossAttackRange = GetComponentInParent<BossAttack>().GetComponentInChildren<BossAttackRange>();
        warningToBreathDelay = bossDataLoader.bossData.warningToBreathDelay;
        postBreathInterval = bossDataLoader.bossData.postBreathInterval;
        bulletSpawnInterval = bossDataLoader.bossData.bulletSpawnInterval;
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

    public async UniTask BreathAttack(Transform bossTransform)
    {
        if(isOnCooldown) return;

        isOnCooldown = true;

        // 警告アラートを表示
        await bossAttackRange.ShowWarningAlert();

        // 警告アラートが表示されてから何秒後にブレス攻撃を開始するか
        await UniTask.Delay(TimeSpan.FromSeconds(warningToBreathDelay));

        // 指定時間、連続的に弾を発射する
        float breathDuration = bossDataLoader.bossData.breathDuration; // 例: ブレスを2秒間続ける
        float startTime = Time.time;

        while(Time.time - startTime < breathDuration)
        {
            if (activeBulletsCount < maxActiveBullets)
            {
                // 三つの弾を一度に発射
                Vector3 offsetLeft = new Vector3(-1, 0, 0);
                Vector3 offsetRight = new Vector3(1, 0, 0);

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
    }
    private async UniTask MoveBullet(GameObject bullet, Vector3 startPosition, Transform bossTransform)
    {
        bullet.transform.position = startPosition;
        Vector3 direction = (player.transform.position - bossTransform.position).normalized;

        for (float movedDistance = 0; movedDistance < 30f; movedDistance += bulletMoveSpeed * Time.deltaTime)
        {
            bullet.transform.position += direction * bulletMoveSpeed * Time.deltaTime;
            await UniTask.Yield();
        }

        bullet.SetActive(false);
        activeBulletsCount--;
    }
}
