using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class BossScript : MonoBehaviour, IEnemy,IDamaged
{
    private BossData bossData;
    private Subject<Unit> onDestroyed = new Subject<Unit>();
    public IObservable<Unit> OnDestroyed => onDestroyed;
    private CompositeDisposable disposable = new CompositeDisposable();
    private bool playerInRange = false;
    private Vector3 targetPosition;  // プレイヤーの位置
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Color sourceColor;
    private float hp;
    public async UniTask Start()
    {
        bossData = await AddressLoader.AddressLoad<BossData>(AddressableAssetAddress.BOSS_DATA);
        hp = bossData.hp;
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
        Debug.Log("攻撃態勢");
        spriteRenderer.color = new Color(255, 0, 0);
    }
    private void PrepareAttackCancel(){
        Debug.Log("攻撃態勢キャンセル");
        spriteRenderer.color = sourceColor;
    }
    private void BreathAttack()
    {

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
