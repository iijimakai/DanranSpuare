using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System;
using System.Threading.Tasks;

namespace Shun_Player
{
    public class ShockWave : MonoBehaviour
    {
        float damage = 0;

        public void Set(Vector2 pos, Vector2 vec, float charge)
        {
            damage = PlayerParameter.chargeDmg;
            transform.position = pos;
            var angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(charge/14, charge/28, 0);
            transform.localEulerAngles = new Vector3(0, 0, angle);
            transform.localPosition = new Vector3(-vec.x * charge/28 , -vec.y * charge/28, 0);
            gameObject.SetActive(true);
            Delay();
        }

        private async void Delay()
        {
            await Task.Delay(TimeSpan.FromSeconds(PlayerParameter.waveTime));
            gameObject.SetActive(false);
        }

        public void Damage(EnemyBase target)
        {
            target.Damage((int)damage);
            ////Debug.Log("wave : " + damage);
        }
    }
}
