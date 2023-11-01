using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shun_Player
{
    public class IceCollision : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(TagName.Enemy))
            {
                EnemyBase target = null;
                target = target == null? other.gameObject.GetComponent<E1Controller>() : target;
                target = target == null ? other.gameObject.GetComponent<E2Controller>() : target;
                target = target == null ? other.gameObject.GetComponent<E3Controller>() : target;
                target = target == null ? other.gameObject.GetComponent<E4Controller>() : target;

                Debug.Log(target);
                target.Damage((int)PlayerParameter.chargeDmg);
            }
        }
        //public void Damage(EnemyBase target)
        //{
        //    Debug.Log("Hit");
        //    var rigitbody = target.GetComponent<Rigidbody2D>();
        //    Vector2 targetPos = target.transform.position;
        //    Vector2 myPos = transform.position;

        //    var knockBackVector = myPos - targetPos;
        //    rigitbody.AddForce(knockBackVector.normalized);

        //    target.Damage((int)PlayerParameter.chargeDmg);
        //    ////Debug.Log("wave : " + damage);
        //}
    }
}

