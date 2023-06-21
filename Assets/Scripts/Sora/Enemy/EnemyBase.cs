using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Constants;
using System;
using Bullet;
using Cysharp.Threading.Tasks;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EnemyBase : MonoBehaviour
    {
        private int hp;
        private int attackPoint;

        private float speed;
        private bool isMove = true;
        private EnemyData data;

        private Color sorceColor;

        private SpriteRenderer spriteRenderer;

        private Subject<bool> isAttack = new Subject<bool>();
        private Subject<Unit> deadFlag = new Subject<Unit>();

        public CompositeDisposable moveDispose = new CompositeDisposable();
        public CompositeDisposable disposables = new CompositeDisposable();

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="type">エネミータイプ</param>
        public async UniTask Init(EnemyType type)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            sorceColor = spriteRenderer.color;
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
            speed = data.speed;
        }

        /// <summary>
        /// Subscribleの購読を開始する
        /// </summary>
        public void SubscriptionStart(GameObject player)
        {
            this.UpdateAsObservable()
                .Where(_ => IsAattackRange(player))
                .Subscribe(_ => isAttack.OnNext(true))
                .AddTo(disposables);

            this.UpdateAsObservable()
                .Where(_ => !IsAattackRange(player))
                .Subscribe(_ => isAttack.OnNext(false))
                .AddTo(disposables);


            this.UpdateAsObservable()
                .Where(_ => isMove)
                .Subscribe(_ => Move(player))
                .AddTo(moveDispose);

            this.UpdateAsObservable()
                .Subscribe(_ => LockPlayer(player))
                .AddTo(moveDispose);

            deadFlag.Subscribe(_ => Dead())
                .AddTo(disposables);
        }

        /// <summary>
        /// 攻撃範囲内かの判定
        /// </summary>
        /// <param name="player">プレイヤーオブジェクト</param>
        /// <returns>攻撃範囲内か</returns>
        public bool IsAattackRange(GameObject player)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= data.firingRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 動く処理
        /// </summary>
        /// <param name="player">プレイヤーオブジェクト</param>
        public void Move(GameObject player)
        {
            transform.position = Vector3.MoveTowards(transform.position
            , player.transform.position, data.speed * Time.deltaTime);
        }

        /// <summary>
        /// 動く速さを変える
        /// </summary>
        /// <param name="isSpeedChenge">早くするか遅くするか</param>
        public void SpeedChenge(bool isSpeedChenge)
        {
            if (isSpeedChenge)
            {
                speed = data.accelerationSpeed;
            }
            else
            {
                speed = data.speed;
            }
        }

        /// <summary>
        /// プレイヤーの方向を向く
        /// </summary>
        /// <param name="player">プレイヤーオブジェクト</param>
        public void LockPlayer(GameObject player)
        {
            Vector3 toDirection = player.transform.position - transform.position;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);
        }

        /// <summary>
        /// 攻撃間隔を測る
        /// </summary>
        public void AttackInterval()
        {
            isMove = false;
            spriteRenderer.color = new Color(255, 0, 0);
            Observable.Interval(TimeSpan.FromSeconds(data.attackInterval))
                .Subscribe(_ => Attack())
                .AddTo(disposables);
        }

        public void AttackStart()
        {
            spriteRenderer.color = sorceColor;
        }

        public void AttackFalse()
        {
            isAttack.OnNext(false);
        }

        /// <summary>
        /// 弾の初期値を入力
        /// </summary>
        /// <param name="bullet">弾のスクリプト</param>
        public void ShotInit(BulletMove bullet, Transform shotPos)
        {
            bullet.Init(data.attackPoint, data.bulletSpeed, data.firingRange, data.deleteTime, shotPos);
        }

        /// <summary>
        /// すべての購読を停止する
        /// </summary>
        public void DisposableClear()
        {
            isMove = true;
            disposables.Clear();
            moveDispose.Clear();
        }

        /// <summary>
        /// 被弾処理
        /// </summary>
        /// <param name="damage">ダメージ</param>
        public void Damage(int damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                deadFlag.OnNext(Unit.Default);
            }
        }

        /// <summary>
        /// 攻撃範囲を返す
        /// </summary>
        /// <returns>攻撃範囲(射程)</returns>
        public float GetAttackRange()
        {
            return data.firingRange;
        }

        /// <summary>
        /// 瞬間移動できる距離を渡す
        /// </summary>
        /// <returns>距離</returns>
        public float GetTeleportationDistance()
        {
            return data.teleportationDistance;
        }

        public IObservable<bool> GetIsAttack()
        {
            return isAttack;
        }

        public IObservable<Unit> GetDeadFlag()
        {
            return deadFlag;
        }

        public abstract void Spawn();
        public abstract void Attack();
        public abstract void Dead();

        private void OnDestroy()
        {
            disposables.Dispose();
        }

        private void OnDisable()
        {
            disposables.Clear();
        }
    }
}