using Bullet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shun_Rod
{
    using parameter = Shun_Player.PlayerParameter;
    public class Rod1 : RodBase
    {
        [SerializeField] private GameObject shotPos;    
        private float timeCounter = 0;
        public override void AttackInterval()
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= parameter.rodInterval)
            {
                timeCounter = 0;
                Shot(SearchNearEnemy("Enemy"));
            }
        }

        private async void Shot(Transform target)
        {
            Vector3 toDirection = target.transform.position - shotPos.transform.position;
            shotPos.transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);

            GameObject bullet = await BulletPoolUtile.GetBullet(AddressableAssetAddress.SYS_BULLET);
            ShotInit(bullet.GetComponent<BulletMove>(), shotPos.transform);
        }
    }
}
