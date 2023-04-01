using UnityEngine;
using Sora_Constants;
using Cysharp.Threading.Tasks;
using Sora_Bullet;
using Bullet;

namespace Sora_Enemy
{
    public class E2Controller : EnemyBase
    {
        private GameObject player;
        private BulletPool pool;
        private async void Awake()
        {
            player = GameObject.FindGameObjectWithTag(TagName.Player);
            pool = GameObject.FindGameObjectWithTag(TagName.BulletPoolParent).GetComponent<BulletPool>();
            await base.Init(EnemyType.E2);
            await UniTask.WaitUntil(() => player != null);
        }

        private void OnBecameVisible()
        {
            Spawn();
        }

        private void OnBecameInvisible()
        {
            DisposableClear();
        }

        /// <summary>
        /// エネミー表示時に実行
        /// </summary>
        public override void Spawn()
        {
            base.SubscriptionStart(player);
            AttackInterval();
        }

        /// <summary>
        /// 攻撃処理
        /// </summary>
        public override void Attack()
        {
            GameObject bullet = pool.GetBullet(transform);
            base.ShotInit(bullet.GetComponent<BulletMove>(), transform);
        }

        /// <summary>
        /// 死亡処理
        /// </summary>
        public override void Dead()
        {
            // TODO: Pool完成時に追記
        }
    }
}