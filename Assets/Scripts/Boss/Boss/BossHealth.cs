using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections;

public class BossHealth : MonoBehaviour, IEnemy, IDamaged
{
    private float hp;
    private Subject<Unit> onDestroyed = new Subject<Unit>();
    public IObservable<Unit> OnDestroyed => onDestroyed;

    public void InitializeHealth(float initialHp)
    {
        this.hp = initialHp;
    }

    public void Damage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
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
