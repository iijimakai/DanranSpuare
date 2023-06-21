using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseClickController : MonoBehaviour
{
    [SerializeField] private GameObject punikon;
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            punikon.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            punikon.SetActive(false);
        }
    }
}
