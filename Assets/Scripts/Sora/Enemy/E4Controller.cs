using UnityEngine;
using Sora_Constants;
using Lean.Pool;

namespace Sora_Enemy
{
    public class E4Controller : EnemyBase
    {
        private GameObject player;

        private async void Start()
        {
            player = GameObject.FindGameObjectWithTag(TagName.Player);
            Debug.Log(player.name);
            await Init(EnemyType.E4);
            Spawn();
        }

        private void OnBecameVisible()
        {
            Attack();
        }

        private void OnBecameInvisible()
        {
            SpeedChenge(false);
        }

        /// <summary>
        /// 生成時の処理
        /// </summary>
        public override void Spawn()
        {
            SubscriptionStart(player);
        }
        /// <summary>
        /// 攻撃処理
        /// </summary>
        public override void Attack()
        {
            SpeedChenge(true);
        }
        /// <summary>
        /// 死亡処理
        /// </summary>
        public override void Dead()
        {
            // TODO: 死亡エフェクト
            LeanPool.Despawn(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: 攻撃処理
            Debug.Log("攻撃処理");
            Dead();
        }
    }
}