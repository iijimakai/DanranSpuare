using UnityEngine;
using Constants;
using Cysharp.Threading.Tasks;
using Bullet;
using Lean.Pool;
using System;
using UniRx;
using UniRx.Triggers;
using System.Threading.Tasks;

namespace Enemy
{
    public class E2Controller : EnemyBase,IEnemy
    {
        private Subject<Unit> onDestroyed = new Subject<Unit>();
        public IObservable<Unit> OnDestroyed => onDestroyed;

        private GameObject player;
        private async void Awake()
        {
            await Task.Delay(500);
            Debug.Log("Start");
            player = GameObject.FindGameObjectWithTag(TagName.Player);
            await base.Init(EnemyType.E2);
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
        public override async void Spawn()
        {
            await UniTask.WaitUntil(() => player != null);
            SubscriptionStart(player);
            AttackInterval();
        }

        /// <summary>
        /// 攻撃処理
        /// </summary>
        public override async void Attack()
        {
            GameObject bullet = await BulletPoolUtile.GetBullet(AddressableAssetAddress.E2_BULLET);
            base.ShotInit(bullet.GetComponent<BulletMove>(), transform);
        }

        /// <summary>
        /// 死亡処理
        /// </summary>
        public override void Dead()
        {
            // TODO: Pool完成時に追記
            Debug.Log("Daed");
            base.DisposableClear();
            DestroyEnemy();
            //LeanPool.Despawn(gameObject);
        }
        // void OnCollisionEnter2D(Collision2D col)
        // {
        //     if(col.gameObject.CompareTag("Player"))
        //     {
        //         DestroyEnemy();
        //     }
        // }
            // 敵が破壊されたときに呼ばれる関数
        public void DestroyEnemy()
        {
            onDestroyed.OnNext(Unit.Default);
            //onDestroyed.OnCompleted();

            gameObject.SetActive(false);
        }
        public void ResetSubscription()
        {
            onDestroyed.Dispose();
            onDestroyed = new Subject<Unit>();
        }
    }
}