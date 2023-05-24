using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Bullet;
using System;
using Shun_Player;

namespace Shun_Rod
{
    using parameter = Shun_Player.PlayerParameter;
    public abstract class RodBase : MonoBehaviour
    {
        public float hp { get; private set; }

        private CompositeDisposable disposables = new CompositeDisposable();

        private void Start()
        {
            
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
            bullet.Init((int)parameter.rodDmg, parameter.shotSpeed, parameter.rodRange, parameter.rodDuration, shotPos);
        }

        public void Damage(int damage)
        {

        }

        public Transform SearchNearEnemy(string tagName)
        {
            var targets = GameObject.FindGameObjectsWithTag(tagName);
            if (targets.Length == 1) return targets[0].transform;

            GameObject result = null;
            var minTargetDistance = float.MaxValue;
            foreach (var target in targets)
            {
                var targetDistance = Vector3.Distance(transform.position, target.transform.position);
                if (!(targetDistance < minTargetDistance)) continue;
                minTargetDistance = targetDistance;
                result = target.transform.gameObject;
            }

            return result?.transform;
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
