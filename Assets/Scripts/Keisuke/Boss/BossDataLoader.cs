using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections;

public class BossDataLoader : MonoBehaviour
{
    public BossData bossData;

    public async UniTask LoadBossData()
    {
        bossData = await AddressLoader.AddressLoad<BossData>(AddressableAssetAddress.BOSS_DATA);
    }
}
