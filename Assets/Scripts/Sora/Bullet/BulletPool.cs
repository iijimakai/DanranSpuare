using UnityEngine;
using Sora_System;
using Sora_Constants;
using System.Collections.Generic;
using UniRx;
using System;

namespace Sora_Bullet
{
    public class BulletPool : ObjectPoolBase
    {
        [SerializeField, Header("誰の弾か")] private BulletType type;
        private BulletPoolData data;
        private GameObject createObj;
        private List<GameObject> retrievedBulletList = new List<GameObject>();

        private Subject<Unit> createEnd = new Subject<Unit>();

        private void Awake()
        {
            ObjectPool();
        }
        /// <summary>
        /// Poolを開始する
        /// </summary>
        private async void ObjectPool()
        {
            data = await AddressLoader.AddressLoad<BulletPoolData>(AddressableAssetAddress.BULLET_POOL_DATA);
            createObj = await AddressLoader.AddressLoad<GameObject>(GetCreateAddress());
            for (int i = 0; i < GetMaxvalue(); i++)
            {
                base.Create(createObj);
            }
            createEnd.OnNext(Unit.Default);
        }

        /// <summary>
        /// Poolを取り出す
        /// </summary>
        /// <param name="shotPos">発射位置</param>
        public GameObject GetBullet(Transform shotPos)
        {
            GameObject bullet = base.GetCreateObj(shotPos);
            retrievedBulletList.Add(bullet);
            return bullet;
        }

        /// <summary>
        /// Poolをしまう
        /// </summary>
        /// <param name="bullet">取り出したオブジェクト</param>
        public void RemoveBullet(GameObject bullet)
        {
            retrievedBulletList.Remove(bullet);
            base.DeleteObj(bullet);
        }

        public IObservable<Unit> GetCreateEnd()
        {
            return createEnd;
        }

        /// <summary>
        /// 弾のアドレスを取得
        /// </summary>
        /// <returns></returns>
        private string GetCreateAddress()
        {
            switch (type)
            {
                case BulletType.PlayerTem:
                    return AddressableAssetAddress.TEM_BULLET;
                case BulletType.PlayerLag:
                    return AddressableAssetAddress.LAG_BULLET;
                case BulletType.PlayerSys:
                    return AddressableAssetAddress.SYS_BULLET;
                case BulletType.EnemyE1:
                    return AddressableAssetAddress.E1_BULLET;
                case BulletType.EnemyE2:
                    return AddressableAssetAddress.E2_BULLET;
                case BulletType.EnemyE3:
                    return AddressableAssetAddress.E3_BULLET;
                case BulletType.EnemyE4:
                    return AddressableAssetAddress.E4_BULLET;
            }
            return AddressableAssetAddress.E1_BULLET;
        }

        /// <summary>
        /// Poolの最大数をとる
        /// </summary>
        /// <returns>最大数</returns>
        private int GetMaxvalue()
        {
            switch (type)
            {
                case BulletType.PlayerTem:
                    return data.playerTemBulletMaxValue;
                case BulletType.PlayerLag:
                    return data.playerLagBulletMaxValue;
                case BulletType.PlayerSys:
                    return data.playerSysBulletMaxValue;
                case BulletType.EnemyE1:
                    return data.enemyE1BulletMaxValue;
                case BulletType.EnemyE2:
                    return data.enemyE2BulletMaxValue;
                case BulletType.EnemyE3:
                    return data.enemyE3BulletMaxValue;
                case BulletType.EnemyE4:
                    return data.enemyE4BulletMaxValue;
            }
            return 0;
        }
    }
}