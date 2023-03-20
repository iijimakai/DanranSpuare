using UnityEngine;
using Sora_Constants;
using Sora_Bullet;
using Bullet;
using UniRx;
using UniRx.Triggers;

namespace Sora_Enemy
{
    public class E1Controller : EnemyBase
    {
        private GameObject player;
        [SerializeField] private GameObject shotPos;
        [SerializeField] private BulletPool pool;

        // TODO: EnemyPool完成時に変更
        /// <summary>
        /// Instantiate時に実行
        /// </summary>
        public void Awake()
        {
            player = GameObject.FindGameObjectWithTag(TagName.Player);
            this.UpdateAsObservable()
                .Subscribe(_ => TargetLockShotPos())
                .AddTo(this);

            pool.GetCreateEnd()
                .Subscribe(_ => Spawn())
                .AddTo(this);
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
        public async override void Spawn()
        {
            await base.Init(EnemyType.E1);
            base.AttackInterval();
        }

        public override void Dead()
        {
            Debug.Log("Daed");
        }

        /// <summary>
        /// 攻撃
        /// </summary>
        public override void Attack()
        {
            GameObject bullet = pool.GetBullet(transform);
            base.ShotInit(bullet.GetComponent<BulletMove>(), shotPos.transform);
        }
    }
}