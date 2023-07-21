using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class SendDead : MonoBehaviour
{
    private CompositeDisposable disposables = new CompositeDisposable();
    public Subject<Unit> OnDestroyed = new Subject<Unit>();
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            DestroyEnemy();
        }
    }
    // “G‚ª”j‰ó‚³‚ê‚½‚Æ‚«‚ÉŒÄ‚Î‚ê‚éŠÖ”
    public void DestroyEnemy()
    {
        OnDestroyed.OnNext(Unit.Default);
        OnDestroyed.OnCompleted();

        gameObject.SetActive(false);
    }
    public void SubscriptionReset()
    {
        OnDestroyed = new Subject<Unit>();
    }
}
