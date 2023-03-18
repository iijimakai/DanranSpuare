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