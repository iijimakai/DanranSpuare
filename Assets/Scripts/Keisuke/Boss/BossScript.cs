using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class BossScript : MonoBehaviour, IEnemy,IDamaged
{
    private BossData bossData;
    private float attackCooldown;
    private float prepareAttackCooldown; // 攻撃態勢のクールダウン
    private Subject<Unit> onDestroyed = new Subject<Unit>();
    public IObservable<Unit> OnDestroyed => onDestroyed;
    private CompositeDisposable disposable = new CompositeDisposable();
    private bool playerInRange = false;
    private bool isFirstAttack = true; // 一発目の攻撃かどうか
    private Vector3 targetPosition;  // プレイヤーの位置
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private float hp;
    public async UniTask Init()
    {
        bossData = await AddressLoader.AddressLoad<BossData>(AddressableAssetAddress.BOSS_DATA);
        attackCooldown = bossData.firstAttackInterval;
        prepareAttackCooldown = bossData.prepareAttackInterval;
        hp = bossData.hp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CheckAttackRange();
        player = GameObject.FindGameObjectWithTag(TagName.Player);
    }
    private void CheckAttackRange()
    {
        this.UpdateAsObservable()
        .Subscribe(_ =>
        {
            TrackingPlayerMove();
            if (playerInRange)
            {
                if (prepareAttackCooldown <= 0)
                {
                    // 攻撃
                    if (attackCooldown <= 0)
                    {
                        Attack();
                        // FirstAttackIntervalの時間だけ攻撃を遅らせ、その後は通常の攻撃間隔
                        attackCooldown = isFirstAttack ? bossData.firstAttackInterval : bossData.attackInterval;
                        isFirstAttack = false;
                    }
                    else
                    {
                        attackCooldown -= Time.deltaTime;
                    }
                }
                else
                {
                    PrepareAttack();
                    prepareAttackCooldown -= Time.deltaTime;
                }
            }
        }).AddTo(disposable);
    }
    public void SetPlayerInRange(bool inRange)
    {
        this.playerInRange = inRange;
    }
    public void SetTargetPosition(Vector3 position)
    {
        this.targetPosition = position;
    }
    private void PrepareAttack()
    {
        Debug.Log("攻撃態勢");
        spriteRenderer.color = new Color(255, 0, 0);
    }
    private void Attack()
    {
        float step = bossData.moveSpeed * Time.deltaTime;  // 移動スピード
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        Debug.Log("BossAttack");
    }
    private void TrackingPlayerMove()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance >= bossData.trackingRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 1f * Time.deltaTime);
        }
    }
    public void Damage(int damage)
    {
        Debug.Log("BOSS"+hp +"->"+ (hp - damage));
        hp -= damage;
        if(hp < 0)
        {
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        onDestroyed.OnNext(Unit.Default);
        gameObject.SetActive(false);
    }
    public void ResetSubscription()
    {
        onDestroyed.Dispose();
        onDestroyed = new Subject<Unit>();
    }
}
