using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public JoystickController joystickController;
    public float speed = 5f;

    void Update()
    {
        Vector2 direction = joystickController.GetInputDirection();
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
