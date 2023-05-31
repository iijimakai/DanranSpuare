using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
// IEnemyインターフェースを定義
public interface IEnemy
{
    IObservable<Unit> OnDestroyed { get; }
    void ResetSubscription();
}
public class Enemy4Controller : MonoBehaviour,IEnemy
{
    public float speed;
    [SerializeField] private GameObject playerObject;
    private CompositeDisposable disposables = new CompositeDisposable();
    private Subject<Unit> onDestroyed = new Subject<Unit>();
    public IObservable<Unit> OnDestroyed => onDestroyed;
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerTracking(playerObject);
    }

    public void PlayerTracking(GameObject targetObject)
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                Vector3 targetDirection = (targetObject.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                transform.position += transform.up * speed * Time.deltaTime;
            }).AddTo(disposables);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            DestroyEnemy();
        }
    }

    public void Damage()
    {
        Debug.Log("Dead");
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
