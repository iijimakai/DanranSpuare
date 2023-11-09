using UnityEngine;
using Constants;
using Cysharp.Threading.Tasks;
using Bullet;
using Lean.Pool;
using System;
using UniRx;
using UniRx.Triggers;

namespace Enemy
{
    public class E2Controller : EnemyBase,IEnemy,IDamaged
    {
        private Subject<Unit> onDestroyed = new Subject<Unit>();
        public IObservable<Unit> OnDestroyed => onDestroyed;
        private GameObject player;
        [SerializeField] private GameObject shotPos;
        private async UniTaskVoid Awake()
        {
            var cancellationToken = this.GetCancellationTokenOnDestroy();
            try
            {
                await UniTask.Delay(500);
                player = GameObject.FindGameObjectWithTag(TagName.Player);
                await base.Init(EnemyType.E2).AttachExternalCancellation(cancellationToken);
                StartSubscriptions();
            }
            catch (OperationCanceledException)
            {
                // キャンセル時の処理
                Debug.Log("E2 Initialization was canceled due to the MonoBehaviour being destroyed.");
            }
        }
        private void OnBecameVisible()
        {
            Spawn();
        }

        private void OnBecameInvisible()
        {
            DisposableClear();
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
            if (player == null || shotPos == null)
            {
                return;
            }
            Vector3 direction = player.transform.position - shotPos.transform.position;
            shotPos.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
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
            base.ShotInit(bullet.GetComponent<BulletMove>(), shotPos.transform);
        }
        public void Damage(int damage)
        {
            //Debug.Log("E2"+hp +"->"+ (hp - damage));
            hp -= damage;
            if(hp < 0)
            {
                Dead();
            }
            ColorChange();
        }
        /// <summary>
        /// 死亡処理
        /// </summary>
        public override void Dead()
        {
            // TODO: Pool完成時に追記
            Debug.Log("DaedE2");
            base.DisposableClear();
            DestroyEnemy();
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
            if (data != null)
            {
                hp = data.hp;
            }
            else
            {
                Debug.LogWarning("Attempted to reset subscriptions on an uninitialized enemy.");
            }
        }
        private void OnDestroy()
        {
            // すべての非同期操作をキャンセル
            disposables.Clear();
        }
    }
}