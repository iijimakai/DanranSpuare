using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskHMeter : MonoBehaviour
{
    [SerializeField] float Value;
    [SerializeField] Vector2 movelength;
    // Start is called before the first frame update
    void Start()
    {
        movelength.x =210.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position.x = tagPos.x+(movelength.x*Value);
        transform.localPosition = new(250.0f-(movelength.x*(Value-0.5f)),transform.localPosition.y,transform.localPosition.z) ;
    }
}
