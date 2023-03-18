using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Bullet
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        private int attackPoint;
        private float bulletSpeed;
        private float firingRange;
        [SerializeField, Header("敵かプレイヤーか"), Tooltip("プレイヤーならチェックを入れない")] private bool isEnemy;

        private Vector3 initalPosition = new Vector3();

        private Subject<Unit> isRangeOutSide = new Subject<Unit>();
        private CompositeDisposable disposables = new CompositeDisposable();

        /// <summary>
        /// 生成時の初期化
        /// </summary>
        /// <param name="_attackPoint">攻撃力</param>
        /// <param name="_bulletSpeed">スピード</param>
        /// <param name="_firingRange">射程</param>
        public void Init(int _attackPoint, float _bulletSpeed, float _firingRange)
        {
            attackPoint = _attackPoint;
            bulletSpeed = _bulletSpeed;
            firingRange = _firingRange;
            isRangeOutSide.Subscribe(_ => RemoveBullet())
                .AddTo(this);
        }

        /// <summary>
        /// 弾の発射
        /// </summary>
        /// <param name="parentPos">発射位置</param>
        public void Shot(Transform parentPos)
        {
            transform.position = parentPos.position;
            transform.localEulerAngles = parentPos.localEulerAngles;
            initalPosition = transform.position;
            this.UpdateAsObservable()
                .Subscribe(_ => BulletMove())
                .AddTo(disposables);
        }

        /// <summary>
        /// 弾の動き
        /// </summary>
        private void BulletMove()
        {
            transform.Translate(bulletSpeed * Time.deltaTime, 0, 0);
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
                    RemoveBullet();
                }
            }
            else
            {
                if (other.CompareTag(TagName.Enemy))
                {
                    //TODO: エネミーの被弾処理
                    RemoveBullet();
                }
            }
        }

        /// <summary>
        /// 弾をPoolに戻す
        /// </summary>
        private void RemoveBullet()
        {
            disposables.Clear();
            //TODO: ObjectPoolを実装したら追加
        }
    }
}