using UnityEngine;
using Constants;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;

namespace Enemy
{
    public class E3Controller : EnemyBase
    {
        [SerializeField] private GameObject attackObj;
        private GameObject player;

        private async void Awake()
        {
            // TODO: EnemyPoolができたら変更
            attackObj.SetActive(false);
            player = GameObject.FindGameObjectWithTag(TagName.Player);
            await base.Init(EnemyType.E3);
            await UniTask.WaitUntil(() => player != null);
            Spawn();
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
            attackObj.SetActive(true);
            transform.position += transform.TransformDirection(0, base.GetTeleportationDistance(), 0);
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
            await UniTask.WaitUntil(() => !attackObj.activeSelf);
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

        public override void Dead()
        {
            base.DisposableClear();
            Debug.Log("Daed");
            LeanPool.Despawn(gameObject);
        }
    }
}