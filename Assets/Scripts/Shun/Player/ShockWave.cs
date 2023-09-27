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
        public void Set(Vector2 pos, Vector2 vec)
        {
            var angle = Mathf.Atan2(-vec.y, -vec.x) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0, 0, angle);
            gameObject.SetActive(true);
            Delay();
        }

        private async void Delay()
        {
            await Task.Delay(TimeSpan.FromSeconds(PlayerParameter.waveTime));
            gameObject.SetActive(false);
        }
    }
}
