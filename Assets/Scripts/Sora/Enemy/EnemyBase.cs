using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sora_Constants;
using System;
using Bullet;
using Cysharp.Threading.Tasks;

namespace Sora_Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EnemyBase : MonoBehaviour
    {
        private EnemyData data;

        private Subject<Unit> attack = new Subject<Unit>();

        private CompositeDisposable disposables = new CompositeDisposable();

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="type">エネミータイプ</param>
        public async UniTask Init(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.E1:
                    data = await AddressLoader.AddressLoad<EnemyData>(AddressableAssetAddress.E1_DATA);
                    break;
                case EnemyType.E2:
                    data = await AddressLoader.AddressLoad<EnemyData>(AddressableAssetAddress.E2_DATA);
                    break;
                case EnemyType.E3:
                    data = await AddressLoader.AddressLoad<EnemyData>(AddressableAssetAddress.E3_DATA);
                    break;
                case EnemyType.E4:
                    data = await AddressLoader.AddressLoad<EnemyData>(AddressableAssetAddress.E4_DATA);
                    break;
            }
            Debug.Log(data);
        }

        /// <summary>
        /// 動く処理
        /// </summary>
        public void Move()
        {
            //TODO: 後で実装
        }

        public void AttackInterval()
        {
            Observable.Interval(TimeSpan.FromSeconds(data.attackInterval))
                .Subscribe(_ => Attack())
                .AddTo(disposables);
        }

        /// <summary>
        /// 弾の初期値を入力
        /// </summary>
        /// <param name="bullet">弾のスクリプト</param>
        public void ShotInit(BulletMove bullet)
        {
            Debug.Log(bullet);
            bullet.Init(data.attackPoint, data.bulletSpeed, data.firingRange, transform);
        }

        private void OnDisable()
        {
            disposables.Clear();
        }

        public abstract void Spawn();
        public abstract void Attack();

        /// <summary>
        /// オブジェクトが消えたときの処理を強制
        /// </summary>
        private void OnDestroy()
        {
            disposables.Dispose();
        }
    }
}