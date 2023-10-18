using Bullet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shun_Rod
{
    using parameter = Shun_Player.PlayerParameter;
    public class Rod2 : RodBase
    {
        [SerializeField] private GameObject shotPos;
        private float timeCounter = 0;
        public override void AttackInterval()
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= parameter.rodInterval)
            {
                timeCounter = 0;
                Shot(CheckRange(SearchNearEnemy(TagName.Enemy), parameter.rodRange));
            }
        }

        private async void Shot(Transform target)
        {
            if (target == null) return;
            Vector3 toDirection = target.transform.position - shotPos.transform.position;
            shotPos.transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);

            GameObject bullet = await BulletPoolUtile.GetBullet(AddressableAssetAddress.TEM_BULLET);
            ShotInit(bullet.GetComponent<BulletMove>(), shotPos.transform);
        }
    }
}
