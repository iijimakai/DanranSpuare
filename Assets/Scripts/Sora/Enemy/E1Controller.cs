using UnityEngine;
using Sora_Constants;
using Bullet;
using UniRx;
using UniRx.Triggers;

namespace Sora_Enemy
{
    public class E1Controller : EnemyBase
    {
        private GameObject player;
        [SerializeField] private GameObject shotPos;

        private CompositeDisposable disposables = new CompositeDisposable();

        // TODO: EnemyPool完成時に変更
        /// <summary>
        /// Instantiate時に実行
        /// </summary>
        public async void Awake()
        {
            player = GameObject.FindGameObjectWithTag(TagName.Player);

            await base.Init(EnemyType.E1);
        }

        private void OnEnable()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => TargetLockShotPos())
                .AddTo(disposables);
        }

        /// <summary>
        /// Playerの方向を向く
        /// </summary>
        private void TargetLockShotPos()
        {
            Vector3 direction = player.transform.position - shotPos.transform.position;
            shotPos.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        }

        /// <summary>
        /// 攻撃間隔の開始
        /// </summary>
        public override void Spawn()
        {
            base.SubscriptionStart(player);
            base.AttackInterval();
        }

        public override void Dead()
        {
            Debug.Log("Daed");
            disposables.Clear();
        }

        /// <summary>
        /// 攻撃
        /// </summary>
        public override async void Attack()
        {
            GameObject bullet = await BulletPoolUtile.GetBullet(AddressableAssetAddress.E1_BULLET);
            base.ShotInit(bullet.GetComponent<BulletMove>(), shotPos.transform);
        }
    }
}