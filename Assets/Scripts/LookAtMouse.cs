using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    private Vector3 BowPos;
    
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public int bulletSpeedMult = 10;
    private float curTimeout;
    private float bulletSpeed;
    private Vector3 targetPos;
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            
        }
        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                bulletSpeed = Mathf.Abs(BowPos.x - hit.point.x) + Mathf.Abs(BowPos.z - hit.point.z);
                Debug.Log("Speed = "+ Mathf.Abs(BowPos.x - hit.point.x) + Mathf.Abs(BowPos.z - hit.point.z));
                if (Mathf.Abs(BowPos.x - hit.point.x) > 0.1 || Mathf.Abs(BowPos.z - hit.point.z) > 0.1)
                {
                    Vector3 HitPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    targetPos = HitPos;
                    transform.LookAt(HitPos);
                }
                else
                {
                    //transform.rotation = Quaternion.LookRotation(Vector3.back);
                }
            }
        }
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
                }
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            GameObject bulletInstance = Instantiate(bulletPrefab,gunPoint.position,gunPoint.rotation) as GameObject;
            Rigidbody instBulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
            instBulletRigidbody.centerOfMass = new Vector3(0,0,0.5f);
            instBulletRigidbody.AddForce(transform.forward * bulletSpeed * bulletSpeedMult*-1);
            
        }
    }


}
