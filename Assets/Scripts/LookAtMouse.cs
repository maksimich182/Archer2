using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Look());
        }
    }

    IEnumerator Look()
    {
        yield return new WaitForSeconds(0.00001f);
        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        if (mousePosition.z <= 1 && mousePosition.x >= -1 && mousePosition.x <= 1)
        {
            while (Input.GetButton("Fire1"))
            {
                mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 xzPos = mousePosition - transform.position;
                xzPos.y = 0;
                var angle = Vector3.Angle(Vector3.back, xzPos);
                if(tag == "Arrow")
                {
                    Debug.Log(transform.position);
                    transform.position = new Vector3(mousePosition.x, 0f, mousePosition.z);
                    

                    Debug.Log(mousePosition);
                }
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, mousePosition.x > 0 ? angle : -angle);
                yield return new WaitForSeconds(0.00001f);
            }
        }
    }

}
