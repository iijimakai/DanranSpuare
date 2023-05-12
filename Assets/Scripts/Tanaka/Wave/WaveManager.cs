using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class WaveManager : MonoBehaviour
{
    public string SceneName;
    [SerializeField] private ObjectController DeathCounter;

    void Start()
    {
        DeathCounter.OnEnemyDeath();
        DeathCounter.OnDeathCountChanged.Subscribe(OnDeathCountRecieved);
    }
    public void OnDeathCountRecieved(int deathcount)
    {
        Debug.Log("OnRecieved");
        if(deathcount > 4)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
