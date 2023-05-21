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
                Vector3 toDirection = targetObject.transform.position - transform.position;
                transform.rotation = Quaternion.FromToRotation(-Vector3.up, toDirection);
                transform.position -= transform.up * speed * Time.deltaTime;
            }).AddTo(disposables);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            DestroyEnemy();
        }
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
