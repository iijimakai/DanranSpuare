using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sora_Constants;

namespace Spra_Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EnemyBase : MonoBehaviour
    {
        private EnemyData data;

        private CompositeDisposable disposables = new CompositeDisposable();

        private void Init(EnemyData enemydata)
        {
            data = enemydata;
            switch (data.enemyType)
            {
                case EnemyType.E1:
                case EnemyType.E2:
                    break;
                case EnemyType.E3:
                case EnemyType.E4:
                    this.UpdateAsObservable()
                        .Subscribe(_ => Move())
                        .AddTo(disposables);
                    break;
            }
        }

        /// <summary>
        /// 動く処理
        /// </summary>
        private void Move()
        {
            //TODO: 後で実装
        }

        /// <summary>
        /// ゲームオブジェクトが破棄されたら購読を停止する。。
        /// </summary>
        private void OnDestroy()
        {
            disposables.Dispose();
        }
    }
}