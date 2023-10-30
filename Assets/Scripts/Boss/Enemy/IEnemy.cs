using System;
using UniRx;

public interface IEnemy
{
    IObservable<Unit> OnDestroyed { get; }
    void ResetSubscription();
}
