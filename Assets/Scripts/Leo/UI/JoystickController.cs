using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform joystickHandle;
    private Vector2 inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragPosition = eventData.position;

        inputVector = (dragPosition - (Vector2)joystickHandle.position) / ((RectTransform)joystickHandle.parent).sizeDelta;
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        joystickHandle.anchoredPosition = inputVector * ((RectTransform)joystickHandle.parent).sizeDelta / 2.0f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    public Vector2 GetInputDirection()
    {
        return inputVector;
    }
}
