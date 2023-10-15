using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections;

public class BossMovement : MonoBehaviour
{
    private float moveSpeed;
    private GameObject player;
    private float trackingRange;

    private void Start()
    {
        await WaitForPlayerSpawn();
        player = GameObject.FindGameObjectWithTag(TagName.Player);
    }
    private async UniTask WaitForPlayerSpawn()
    {
        while (GameObject.FindGameObjectWithTag(TagName.Player) == null)
        {
            await UniTask.Delay(500); // 0.5秒ごとに再試行
        }
    }
    public void InitializeMovementData(float moveSpeed, float trackingRange)
    {
        this.moveSpeed = moveSpeed;
        this.trackingRange = trackingRange;
    }

    public void TrackingPlayerMove()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance >= trackingRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 1f * Time.deltaTime);
        }
    }
}
