using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class BossController : BossScript
{
    private async void Start()
    {
        await base.Init();
    }
}