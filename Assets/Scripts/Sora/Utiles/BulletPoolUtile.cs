using UnityEngine;
using Lean.Pool;
using Cysharp.Threading.Tasks;

public class BulletPoolUtile
{
    private static GameObject bulletObj;
    /// <summary>
    /// Poolを取り出す
    /// </summary>
    /// <param name="address"> Objectのアドレス</param>
    /// <returns>GameObject</returns>
    public static async UniTask<GameObject> GetBullet(string address)
    {
        if (bulletObj == null)
        {
            bulletObj = await AddressLoader.AddressLoad<GameObject>(address);
        }
        GameObject bullet = LeanPool.Spawn(bulletObj);
        return bullet;
    }

    /// <summary>
    /// Poolをしまう
    /// </summary>
    /// <param name="bullet">取り出したオブジェクト</param>
    public static void RemoveBullet(GameObject bullet)
    {
        LeanPool.Despawn(bullet);
    }
}