using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
public class EnemySystemtest : MonoBehaviour
{
    public float speed;
    [SerializeField] private GameObject playerObject;
    private CompositeDisposable disposables = new CompositeDisposable();
    public Subject<Unit> OnDestroyed = new Subject<Unit>();
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

    // 敵が破壊されたときに呼ばれる関数
    //public void DestroyEnemy()
    //{
    //    OnDestroyed.OnNext(Unit.Default);
    //    OnDestroyed.OnCompleted();
    //}
}
