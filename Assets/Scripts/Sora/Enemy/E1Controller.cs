using UnityEngine;
using Sora_Constants;
using Sora_Bullet;
using Bullet;
using UniRx;

namespace Sora_Enemy
{
    public class E1Controller : EnemyBase
    {
        private GameObject player;
        private GameObject shotPos;
        [SerializeField] private BulletPool pool;

        // TODO: EnemyPool完成時に変更
        /// <summary>
        /// Instantiate時に実行
        /// </summary>
        public void Awake()
        {
            player = GameObject.FindGameObjectWithTag(TagName.Player);
            pool.GetCreateEnd()
                .Subscribe(_ => Spawn())
                .AddTo(this);
        }

        public async override void Spawn()
        {
            await base.Init(EnemyType.E1);
            base.AttackInterval();
        }

        public override void Attack()
        {
            GameObject bullet = pool.GetBullet(transform);
            base.ShotInit(bullet.GetComponent<BulletMove>());
        }
    }
}