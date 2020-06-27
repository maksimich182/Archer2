using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowComponent : MonoBehaviour
{
    public Vector3 BowPos;
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    BowPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    transform.position = BowPos;
                    Debug.Log("BowPos1");
                }
            }
        }
        
        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (Mathf.Abs(BowPos.x - hit.point.x) > 0.1 || Mathf.Abs(BowPos.z - hit.point.z) > 0.1)
                {
                    Vector3 HitPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    transform.LookAt(BowPos);
                    transform.position = HitPos;
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.back);
                }
            }
        }
        
        if (Input.GetButtonUp("Fire1"))
        {
            transform.position = BowPos;
            //transform.rotation = Quaternion.LookRotation(Vector3.forward);
           // Debug.Log("GetButtonUp");    
        }
    }

    IEnumerator Look()
    {
        yield return new WaitForSeconds(0.00001f);
        
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //  var mousePosition = Input.mousePosition;
        //  mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        
        if (mousePosition.z <= 1 && mousePosition.x >= -1 && mousePosition.x <= 1)
        {
            while (Input.GetButton("Fire1"))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
