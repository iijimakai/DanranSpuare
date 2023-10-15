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
        player = GameObject.FindGameObjectWithTag(TagName.Player);
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
