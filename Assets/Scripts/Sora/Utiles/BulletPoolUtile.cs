using UnityEngine;
using Lean.Pool;
using Cysharp.Threading.Tasks;

public class BulletPoolUtile
{
    /// <summary>
    /// Poolを取り出す
    /// </summary>
    /// <param name="address"> Objectのアドレス</param>
    /// <returns>GameObject</returns>
    public static async UniTask<GameObject> GetBullet(string address)
    {
        GameObject loadObj = await AddressLoader.AddressLoad<GameObject>(address);
        GameObject bullet = LeanPool.Spawn(loadObj);
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