using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
//Enemy4�̃e�X�g�p�폜����
public class ObjectController : MonoBehaviour
{
    //private ReactiveProperty<int> deathcount = new ReactiveProperty<int>(0);
    //public Subject<int> OnDeathCountChanged = new Subject<int>();
    //[SerializeField] private float lifeTime = 3f;
    void Start()
    {
        //OnDeathCountChanged.OnNext(deathcount.Value);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            //OnEnemyDeath();
        }
    }
    //public void OnEnemyDeath()
    //{
    //    deathcount.Value++;
    //    //Debug.Log(deathcount.Value);
    //}
}
