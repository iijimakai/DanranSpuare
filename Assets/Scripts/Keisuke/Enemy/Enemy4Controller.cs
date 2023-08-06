using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Threading.Tasks;

// IEnemyインターフェースを定義
public interface IEnemy
{
    IObservable<Unit> OnDestroyed { get; }
    void ResetSubscription();
}
public class Enemy4Controller : MonoBehaviour,IEnemy,IDamaged
{
    [SerializeField] private float attackRange = 2f;
    private bool isAttacking = false;
    public float speed;
    [SerializeField] private GameObject playerObject;
    private CompositeDisposable disposables = new CompositeDisposable();
    private Subject<Unit> onDestroyed = new Subject<Unit>();
    public IObservable<Unit> OnDestroyed => onDestroyed;
    private async void Start()
    {
        await Task.Delay(500);
        Debug.Log("Start");
        playerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerTracking(playerObject);
    }

    public void PlayerTracking(GameObject targetObject)
    {
        this.UpdateAsObservable()
            .Where(_ => !isAttacking)
            .Subscribe(_ =>
            {
                Vector3 directionToPlayer = (targetObject.transform.position - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, targetObject.transform.position);

                if (distance <= attackRange)
                {
                    Attack();
                }
                else
                {
                    transform.position += directionToPlayer * speed * Time.deltaTime;
                }
            }).AddTo(disposables);
    }
    private async void Attack()
    {
        isAttacking = true;
        // 攻撃処理
        Debug.Log("攻撃");
        await Task.Delay(2000);
        // 攻撃後の処理
        isAttacking = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("inCamera"))
        {
            speed *= 1.5f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("inCamera"))
        {
            speed /= 1.5f;
        }
    }
    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     if(col.gameObject.tag == "Bullet")
    //     {
    //         DestroyEnemy();
    //     }
    // }
    public void Damage(int damage)
    {
        Debug.Log("DeadE4");
        DestroyEnemy();
    }
    // 敵が破壊されたときに呼ばれる関数
    public void DestroyEnemy()
    {
        onDestroyed.OnNext(Unit.Default);
        //onDestroyed.OnCompleted();

        gameObject.SetActive(false);
    }
    public void ResetSubscription()
    {
        onDestroyed.Dispose();
        onDestroyed = new Subject<Unit>();
    }
}
