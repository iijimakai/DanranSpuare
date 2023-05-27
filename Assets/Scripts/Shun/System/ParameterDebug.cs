using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shun_Player;

public class ParameterDebug : MonoBehaviour
{
    [SerializeField] private PlayerBase player;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ParameterBuff.hpLevel.Value++;

            Debug.Log(player.hp + "/" + PlayerParameter.maxHp);
        }
    }
}
