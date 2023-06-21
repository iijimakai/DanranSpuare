using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public JoystickController joystickController;
    public float speed = 5f;
    //[SerializeField] private GameObject punikon;
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
            //Vector3 mousePosition = Input.mousePosition;
           // mousePosition.z = 10f;
            //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            //Debug.Log(mousePosition);
            //Instantiate(punikon, worldPosition, Quaternion.identity);
            //punikon.SetActive(true);
            //if (Input.GetMouseButtonUp(0))
        //     {
        //         punikon.SetActive(false);
        //         Destroy(punikon);
        //     }
        // }

        Vector2 direction = joystickController.GetInputDirection();
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
