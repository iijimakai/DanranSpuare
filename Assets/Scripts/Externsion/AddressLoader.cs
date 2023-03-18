using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

public static class AddressLoader
{
    /// <summary>
    /// アドレスの読み込み
    /// </summary>
    /// <param name="address">Addressableのアドレス</param>
    /// <typeparam name="T">数値型以外の型</typeparam>
    /// <returns>Addressのロード結果</returns>
    public static async UniTask<T> AddressLoad<T>(string address)
    {
        AsyncOperationHandle<T> loader = Addressables.LoadAssetAsync<T>(address);
        await loader.Task;
        return loader.Result;
    }
}
