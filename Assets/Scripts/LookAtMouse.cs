using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public int bulletSpeedMult = 10;
    public float speed = 5;

    private Vector3 _BowPos;
    private bool _moveLock = false;
    private float _curTimeout;
    private float _bulletSpeed;
    private Vector3 _targetPos;
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
                if (transform.position == hit.point || _moveLock == true)
                {
                    _moveLock = true;
                    _bulletSpeed = Mathf.Abs(_BowPos.x - hit.point.x) + Mathf.Abs(_BowPos.z - hit.point.z);
                    Debug.Log("Speed = " + Mathf.Abs(_BowPos.x - hit.point.x) + Mathf.Abs(_BowPos.z - hit.point.z));
                    if (Mathf.Abs(_BowPos.x - hit.point.x) > 0.1 || Mathf.Abs(_BowPos.z - hit.point.z) > 0.1)
                    {
                        Vector3 HitPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        _targetPos = HitPos;
                        transform.LookAt(HitPos);
                    }
                    else
                    {
                        //transform.rotation = Quaternion.LookRotation(Vector3.back);
                    }
                }
                else if (hit.collider.gameObject.layer == 8)
                {
                    transform.position = Vector3.MoveTowards(transform.position, hit.point, speed);
                }
            }
        }

        //transform.position = Vector3.MoveTowards(
        //                new Vector3(transform.position.x, 0f, transform.position.z),
        //                new Vector3(hit.point.x, 0f, hit.point.z),
        //                speed);


        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.collider.gameObject.layer == 8)
        //        {
        //            BowPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        //            transform.position = BowPos;
        //        }
        //    }
        //}
        if (Input.GetButtonUp("Fire1") && _moveLock == true)
        {
            _moveLock = false;
            GameObject bulletInstance = Instantiate(bulletPrefab,gunPoint.position,gunPoint.rotation) as GameObject;
            Rigidbody instBulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
            instBulletRigidbody.centerOfMass = new Vector3(0,0,0.5f);
            instBulletRigidbody.AddForce(transform.forward * _bulletSpeed * bulletSpeedMult*-1);
            
        }
    }


}
