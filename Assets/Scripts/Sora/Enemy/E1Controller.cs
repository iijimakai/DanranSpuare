using UnityEngine;
using Constants;
using Bullet;
using UniRx;
using UniRx.Triggers;
using Lean.Pool;
using System;
using System.Threading.Tasks;

namespace Enemy
{
    public class E1Controller : EnemyBase,IEnemy
    {
        private Subject<Unit> onDestroyed = new Subject<Unit>();
        public IObservable<Unit> OnDestroyed => onDestroyed;

        private GameObject player;
        [SerializeField] private GameObject shotPos;

        // TODO: EnemyPool完成時に変更
        /// <summary>
        /// Instantiate時に実行
        /// </summary>
        public async void Awake()
        {
            await Task.Delay(500);
            Debug.Log("Start");
            player = GameObject.FindGameObjectWithTag(TagName.Player);

            await base.Init(EnemyType.E1);
        }

        private void OnEnable()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => TargetLockShotPos())
                .AddTo(base.disposables);
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
            base.DisposableClear();
            //LeanPool.Despawn(gameObject);
        }
        // void OnCollisionEnter2D(Collision2D col)
        // {
        //     if(col.gameObject.tag == "Player")
        //     {
        //         DestroyEnemy();
        //     }
        // }
        /// <summary>
        /// 攻撃
        /// </summary>
        public override async void Attack()
        {
            GameObject bullet = await BulletPoolUtile.GetBullet(AddressableAssetAddress.E1_BULLET);
            base.ShotInit(bullet.GetComponent<BulletMove>(), shotPos.transform);
        }
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