using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] SpriteRenderer arrow;
    [SerializeField] float arrowScele = 300f;  // –îˆó‚Ì•\¦”{—¦

    private void Start()
    {
        arrow.gameObject.SetActive(false);
    }

    private void Update()
    {
        ArrowMove();
    }

    public void ArrowMove()
    {
        //ğŒ‚ğ’Ç‰Á—\’è
        if (Input.GetMouseButton(0))
        {
            arrow.gameObject.SetActive(true);

            // mouse‚Æplayer‚Ì‹——£‚ğŒvZ‚µ‚Ä‚»‚Ì’·‚³‚ğg—p
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            float scale = (transform.position - pos).magnitude / Screen.width * arrowScele;
            if (scale < 1.20f)
            {
                arrow.transform.localScale = new Vector3(scale, scale, 1);
            }
            if (scale >= 1.20f)
            {
                arrow.transform.localScale = new Vector3(1.20f, 1.20f, 1);
            }

            float angle = Vector3.Angle(transform.position - pos, Vector3.up);

            Vector3 cross = Vector3.Cross(transform.position - pos, Vector3.up);
            if (cross.z > 0)
            {
                angle *= -1;
            }
            arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        if(Input.GetMouseButtonUp(0))
        {
            arrow.gameObject.SetActive(false);
        }
    }
}
