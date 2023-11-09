using UnityEngine;
using Constants;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using System.Threading.Tasks;
using Shun_Player;

namespace Enemy
{
    public class E3Controller : EnemyBase,IEnemy,IDamaged
    {
        private Subject<Unit> onDestroyed = new Subject<Unit>();
        public IObservable<Unit> OnDestroyed => onDestroyed;
        [SerializeField] private GameObject attackObj;
        private GameObject player;
        private async UniTaskVoid Awake()
        {
            var cancellationToken = this.GetCancellationTokenOnDestroy();
            try
            {
                // ゲーム開始から500ミリ秒待機
                await UniTask.Delay(500, cancellationToken: cancellationToken);
                attackObj.SetActive(false);
                player = GameObject.FindGameObjectWithTag(TagName.Player);
                await base.Init(EnemyType.E3).AttachExternalCancellation(cancellationToken); // こうすることで外部からキャンセルトークンをEnemyBaseのInitに付け加えることができる
                await UniTask.WaitUntil(() => player != null, cancellationToken: cancellationToken);
                Spawn();
            }
            catch (OperationCanceledException)
            {
                // キャンセル時の処理
                Debug.Log("Initialization was canceled due to the MonoBehaviour being destroyed.");
            }
        }

        /// <summary>
        /// 攻撃間隔の開始
        /// </summary>
        public override void Spawn()
        {
            base.SubscriptionStart(player);
            base.GetIsAttack()
                .DistinctUntilChanged()
                .Where(flag => flag)
                .Subscribe(_ => base.AttackInterval())
                .AddTo(base.disposables);

            base.GetIsAttack()
                .DistinctUntilChanged()
                .Where(flag => !flag)
                .Subscribe(_ => AttackIntervalClear())
                .AddTo(base.disposables);
        }
        /// <summary>
        /// 攻撃
        /// </summary>
        public override void Attack()
        {
            base.AttackStart();
            // プレイヤーの方向を計算
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            // プレイヤーの方向に移動
            transform.position += directionToPlayer * base.GetTeleportationDistance();
            // attackObjをプレイヤーの方向に向ける
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            attackObj.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));

            // attackObjをE3から一定の距離離れた位置に配置
            float distanceFromE3 = 1.0f; // この値は適宜調整
            attackObj.transform.position = transform.position + directionToPlayer * distanceFromE3;
            //attackObj.SetActive(true);

            // 攻撃オブジェクトがアクティブになっている場合はプレイヤーにダメージを与える
            if (attackObj.activeInHierarchy)
            {
                // プレイヤーにダメージを与える
                PlayerBase playerBase = player.GetComponent<PlayerBase>();
                if (playerBase != null)
                {
                    playerBase.Damage(data.attackPoint);
                }
            }

            // TODO: アニメーションができ次第消す時間を取得して消す。
            Observable.Timer(TimeSpan.FromSeconds(1f))
                .Subscribe(_ => AttackEnd())
                .AddTo(base.disposables);
        }

        /// <summary>
        /// 攻撃解除
        /// </summary>
        private async void AttackIntervalClear()
        {
            await UniTask.WaitUntil(() => attackObj != null && !attackObj.activeSelf);
            // AttackIntervalを止めるため
            base.DisposableClear();
            Spawn();
            base.SubscriptionStart(player);
        }

        private void AttackEnd()
        {
            base.AttackFalse();
            attackObj.SetActive(false);
        }
        public void Damage(int damage)
        {
            //Debug.Log("E3"+hp +"->"+ (hp - damage));
            hp -= damage;
            if(hp < 0)
            {
                Dead();
            }
            ColorChange();
        }
        public override void Dead()
        {
            base.DisposableClear();
            //Debug.Log("DaedE3");
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