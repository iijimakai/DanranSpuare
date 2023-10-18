using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Enemy;
using System;
using Shun_Player;

namespace Bullet
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletMove : MonoBehaviour
    {
        private int attackPoint = 5;
        //private float enemyAttackPoint = 5f;
        private float bulletSpeed;
        private float firingRange;
        private float deleteTime;
        [SerializeField, Header("敵かプレイヤーか"), Tooltip("プレイヤーならチェックを入れない")] private bool isEnemy;

        private Vector3 initalPosition = new Vector3();

        private Subject<Unit> isRangeOutSide = new Subject<Unit>();
        private CompositeDisposable disposables = new CompositeDisposable();
        private CompositeDisposable deleteTimedisposables = new CompositeDisposable();
        //[SerializeField] private PlayerHPController playerHPController;

        /// <summary>
        /// 生成時の初期化
        /// </summary>
        /// <param name="_attackPoint">攻撃力</param>
        /// <param name="_bulletSpeed">スピード</param>
        /// <param name="_firingRange">射程</param>
        public void Init(int _attackPoint, float _bulletSpeed, float _firingRange, float _deleteTime, Transform pos)
        {
            //enemyAttackPoint = _enemyAttackPoint;
            attackPoint = _attackPoint;
            bulletSpeed = _bulletSpeed;
            firingRange = _firingRange;
            deleteTime = _deleteTime;
            isRangeOutSide.Subscribe(_ => RemoveBullet())
                .AddTo(disposables);
            Shot(pos);
        }

        /// <summary>
        /// 画面内に映ったら
        /// </summary>
        private void OnBecameVisible()
        {
            deleteTimedisposables.Clear();
        }

        /// <summary>
        /// 画面外に行ったら
        /// </summary>
        private void OnBecameInvisible()
        {
            Observable.Timer(TimeSpan.FromSeconds(deleteTime))
                .Subscribe(_ => RemoveBullet())
                .AddTo(deleteTimedisposables);
        }


        /// <summary>
        /// 弾の発射
        /// </summary>
        /// <param name="parentPos">発射位置</param>
        public void Shot(Transform parentPos)
        {
            transform.position = parentPos.position;
            transform.localEulerAngles = parentPos.eulerAngles;
            initalPosition = transform.position;
            this.UpdateAsObservable()
                .Subscribe(_ => Move())
                .AddTo(disposables);
        }

        /// <summary>
        /// 弾の動き
        /// </summary>
        private void Move()
        {
            transform.Translate(0, bulletSpeed * Time.deltaTime, 0);
            float currentDistance = Vector3.Distance(initalPosition, transform.position);

            if (firingRange <= Mathf.Abs(currentDistance))
            {
                isRangeOutSide.OnNext(Unit.Default);
            }
        }

        /// <summary>
        /// 当たり判定
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isEnemy)
            {
                if (other.CompareTag(TagName.Player))
                {
                    //TODO: プレイヤーの被弾処理
                    //other.GetComponent<PlayerHPController>().PlayerTakeDamage(enemyAttackPoint);
                    other.GetComponent<PlayerBase>().Damage(attackPoint);
                    RemoveBullet();
                }
            }
            else
            {
                if (other.CompareTag(TagName.Enemy))
                {
                    Debug.Log("Bullet");
                    RemoveBullet();
                    //TODO: エネミーの被弾処理
                    Debug.Log("attackPoint"+attackPoint);
                    other.GetComponent<IDamaged>().Damage(attackPoint);
                }
            }
        }

        /// <summary>
        /// 弾をPoolに戻す
        /// </summary>
        private void RemoveBullet()
        {
            disposables.Clear();
            deleteTimedisposables.Clear();
            //TODO: ObjectPoolを実装したら追加
            BulletPoolUtile.RemoveBullet(gameObject);
        }
    }
}