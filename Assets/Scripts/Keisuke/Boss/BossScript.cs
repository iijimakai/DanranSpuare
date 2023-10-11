using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using pool;
using System.Collections;

public class BossScript : MonoBehaviour, IEnemy,IDamaged
{
    private int maxActiveBullets;
    private float hp;
    private float bulletMoveSpeed;
    private int activeBulletsCount = 0;
    [SerializeField] private BossBulletPool bulletPool;
    private BossData bossData;
    private Subject<Unit> onDestroyed = new Subject<Unit>();
    public IObservable<Unit> OnDestroyed => onDestroyed;
    public Subject<bool> isAttack = new Subject<bool>();
    private CompositeDisposable disposable = new CompositeDisposable();
    private bool playerInRange = false;
    private Vector3 targetPosition;  // プレイヤーの位置
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Color sourceColor;
    public async UniTask Start()
    {
        bulletPool = BossBulletPool.Instance;
        bossData = await AddressLoader.AddressLoad<BossData>(AddressableAssetAddress.BOSS_DATA);
        hp = bossData.hp;
        bulletMoveSpeed = bossData.bulletMoveSpeed;
        maxActiveBullets = bossData.maxActiveBullets;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sourceColor = spriteRenderer.color;
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
                PrepareAttack();
                BreathAttack();
            }
            else
            {
                PrepareAttackCancel();
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
        isAttack.OnNext(true);
        Debug.Log("攻撃態勢");
        spriteRenderer.color = new Color(255, 0, 0);
    }
    private void PrepareAttackCancel(){
        Debug.Log("攻撃態勢キャンセル");
        spriteRenderer.color = sourceColor;
    }
    private void BreathAttack()
    {
        // 現在アクティブなBulletの数が上限に達しているか確認
        if (activeBulletsCount < maxActiveBullets)
        {
            // PoolからBulletを取得
            GameObject bullet = bulletPool.GetObject();

            // Bulletの位置をボスの位置にセット
            bullet.transform.position = transform.position;

            // Bulletのカウンタを増やす
            activeBulletsCount++;

            // Bulletをプレイヤーの方向に動かす
            StartCoroutine(MoveBullet(bullet));
        }
        isAttack.OnNext(false);
    }
    private IEnumerator MoveBullet(GameObject bullet)
    {
        Vector3 direction = (player.transform.position - transform.position).normalized; // ボスからプレイヤーに向かう方向ベクトル

        for (float movedDistance = 0; movedDistance < 30f; movedDistance += bulletMoveSpeed * Time.deltaTime) // moveDistanceは射程
        {
            bullet.transform.position += direction * bulletMoveSpeed * Time.deltaTime;
            yield return null;
        }

        // Bulletが目的地に到達したら、非アクティブにしてPoolに返す
        bullet.SetActive(false);
        activeBulletsCount--; // Bulletのカウンタを減少
    }
    private void ClawAttack()
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
