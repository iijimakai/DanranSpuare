using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class BossScript : MonoBehaviour, IEnemy
{
    public string bossDataAddress;
    private BossData bossData;
    private float attackCooldown;
    private float prepareAttackCooldown; // 攻撃態勢のクールダウン
    private Subject<Unit> onDestroyed = new Subject<Unit>();
    public IObservable<Unit> OnDestroyed => onDestroyed;
    private CompositeDisposable disposable = new CompositeDisposable();
    private bool playerInRange = false;
    private bool isFirstAttack = true; // 一発目の攻撃かどうか
    private Vector3 targetPosition;  // プレイヤーの位置
    private async UniTaskVoid Start()
    {
        bossData = await AddressLoader.AddressLoad<BossData>(bossDataAddress);
        // ボスの初期化コード
        CheckAttackRange();
    }

    private void CheckAttackRange()
    {
        this.UpdateAsObservable()
        .Subscribe(_ =>
        {
            if (playerInRange)
            {
                if (prepareAttackCooldown <= 0)
                {
                    // 攻撃
                    if (attackCooldown <= 0)
                    {
                        Attack();
                        // 一発目と二発目以降でクールダウンが違う
                        attackCooldown = isFirstAttack ? bossData.FirstAttackInterval : bossData.AttackInterval;
                        isFirstAttack = false; // 一発目の攻撃が終了
                    }
                    else
                    {
                        attackCooldown -= Time.deltaTime;
                    }
                }
                else
                {
                    // 攻撃態勢
                    PrepareAttack();
                    prepareAttackCooldown -= Time.deltaTime;
                }
            }
        }).AddTo(disposable);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TagName.Player))
        {
            playerInRange = true;
            prepareAttackCooldown = bossData.PrepareAttackInterval;
            isFirstAttack = true;
            targetPosition = col.transform.position;  // プレイヤーの位置を記録
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void PrepareAttack()
    {
        // 一瞬動きを止めるなどの攻撃態勢のアニメーションや動作
        Debug.Log("攻撃態勢");
    }

    private void Attack()
    {
        // プレイヤーの座標に向かって飛び掛かる動作
        float step = bossData.MoveSpeed * Time.deltaTime;  // 移動スピード
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // 攻撃のアニメーションや他の動作
        Debug.Log("BossAttack");
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
