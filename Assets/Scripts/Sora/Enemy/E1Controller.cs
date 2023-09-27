using UnityEngine;
using Constants;
using Bullet;
using UniRx;
using UniRx.Triggers;
using Lean.Pool;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Enemy
{
    public class E1Controller : EnemyBase,IEnemy,IDamaged
    {
        private Subject<Unit> onDestroyed = new Subject<Unit>();
        public IObservable<Unit> OnDestroyed => onDestroyed;
        private GameObject player;
        [SerializeField] private GameObject shotPos;

        // TODO: EnemyPool完成時に変更
        /// <summary>
        /// Instantiate時に実行
        /// </summary>
        private void Start()
        {
            //spriteRendererE1 = GetComponent<SpriteRenderer>();
            //sorceColorE1 = spriteRendererE1.color;
            Spawn();
            StartSubscriptions();
            //await Task.Delay(500);
        }
        private async void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag(TagName.Player);
            await base.Init(EnemyType.E1);
        }
        private void StartSubscriptions()
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
        public async void Damage(int damage)
        {
            Debug.Log("E1"+hp +"->"+ (hp - damage));
            hp -= damage;
            if(hp < 0)
            {
                Dead();
            }
            ColorChange();
        }
        public override void Dead()
        {
            Debug.Log("DaedE1");
            base.DisposableClear();
            DestroyEnemy();
            //LeanPool.Despawn(gameObject);
        }
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

            gameObject.SetActive(false);
        }
        public void ResetSubscription()
        {
            onDestroyed.Dispose();
            onDestroyed = new Subject<Unit>();
        }
    }
}