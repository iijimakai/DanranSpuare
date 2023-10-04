using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Bullet;
using Shun_Player;

namespace Shun_Rod
{
    using parameter = Shun_Player.PlayerParameter;

    public abstract class RodBase : MonoBehaviour
    {
        public float hp { get; private set; }

        private PlayerBase player;

        public ReactiveProperty<bool> isDead { get; private set; } = new ReactiveProperty<bool>(false);

        private CompositeDisposable disposables = new CompositeDisposable();

        public void Init(PlayerBase owner)
        {
            transform.position = owner.transform.position;
            player = owner;

            hp = parameter.maxRodHp;

            this.UpdateAsObservable()
                .Subscribe(_ => AttackInterval())
                .AddTo(disposables);
        }

        public abstract void AttackInterval();

        public bool IsAattackRange(GameObject enemy)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance <= parameter.rodRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ShotInit(BulletMove bullet, Transform shotPos)
        {
            bullet.Init((int)parameter.rodDmg, parameter.shotSpeed, parameter.rodRange, parameter.rodDuration, parameter.penetrateCount, shotPos);
        }

        public void Damage(int damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                Dead();
            }
        }

        public GameObject SearchNearEnemy(string tagName)
        {
            var targets = GameObject.FindGameObjectsWithTag(tagName);
            if (targets.Length == 1) return targets[0];

            GameObject result = null;
            var minTargetDistance = float.MaxValue;
            foreach (var target in targets)
            {
                var targetDistance = Vector3.Distance(transform.position, target.transform.position);
                if (!(targetDistance < minTargetDistance)) continue;
                minTargetDistance = targetDistance;
                result = target;
            }
            return result;
        }

        public Transform CheckRange(GameObject target, float distance)
        {
            if (target == null) return null;

            var targetDistance = Vector3.Distance(transform.position, target.transform.position);
            if (targetDistance <= distance)
            {
                return target.transform;
            }

            return null;
        }

        private void Dead()
        {
            player.RodClear(this);
        }

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
