using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections;
using pool;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private BossBulletPool bulletPool;
    private int maxActiveBullets;
    private float bulletMoveSpeed;
    private int activeBulletsCount = 0;
    private GameObject player;
    private BossAttackRange bossAttackRange;
    private bool isOnCooldown = false;
    private void Start()
    {
        bulletPool = BossBulletPool.Instance;
        player = GameObject.FindGameObjectWithTag(TagName.Player);
        bossAttackRange = GetComponentInParent<BossAttack>().GetComponentInChildren<BossAttackRange>();
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
        await bossAttackRange.ShowWarningAlert();
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        while (activeBulletsCount < maxActiveBullets)
        {
            GameObject bullet = bulletPool.GetObject();
            bullet.transform.position = bossTransform.position;
            activeBulletsCount++;

            _ = MoveBullet(bullet, bossTransform);

            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
        await UniTask.WaitUntil(() => activeBulletsCount == 0);
        isOnCooldown = false;
    }

    private async UniTask MoveBullet(GameObject bullet, Transform bossTransform)
    {
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
