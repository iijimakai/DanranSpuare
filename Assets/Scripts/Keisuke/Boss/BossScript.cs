using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections;

public class BossScript : MonoBehaviour
{
    private BossDataLoader dataLoader;
    private BossAttack bossAttack;
    private BossHealth bossHealth;
    private BossMovement bossMovement;
    private bool playerInRange = false;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;
    private Color sourceColor;

    private async UniTask Start()
    {
        // Init
        dataLoader = GetComponent<BossDataLoader>();
        bossAttack = GetComponent<BossAttack>();
        bossHealth = GetComponent<BossHealth>();
        bossMovement = GetComponent<BossMovement>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        sourceColor = spriteRenderer.color;

        await dataLoader.LoadBossData();

        bossHealth.InitializeHealth(dataLoader.bossData.hp);
        bossAttack.InitializeAttackData(dataLoader.bossData.bulletMoveSpeed, dataLoader.bossData.maxActiveBullets);
        bossMovement.InitializeMovementData(dataLoader.bossData.moveSpeed, dataLoader.bossData.trackingRange);

        CheckAttackRange();
    }

    private void CheckAttackRange()
    {
        this.UpdateAsObservable()
        .Subscribe(_ =>
        {
            bossMovement.TrackingPlayerMove();
            if (playerInRange)
            {
                bossAttack.PrepareAttack(spriteRenderer);
                bossAttack.BreathAttack(transform);
            }
            else
            {
                bossAttack.PrepareAttackCancel(spriteRenderer, sourceColor);
            }
        });
    }

    public void SetPlayerInRange(bool inRange)
    {
        this.playerInRange = inRange;
    }

    public void SetTargetPosition(Vector3 position)
    {
        this.targetPosition = position;
    }
}
