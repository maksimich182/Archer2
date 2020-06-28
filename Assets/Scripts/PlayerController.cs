using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject arrow;
    public GameObject finishPositionPrefab;
    public Transform gunPoint;
    public int bulletSpeedMult = 10;
    public float speed = 5;

    private bool _moveLock = false;
    private float _bulletSpeed;
    private Vector3 _finishPosition;
    private GameObject _finishPositionMark;
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == 8)
            {
                _finishPosition = hit.point;
                _finishPositionMark = Instantiate(finishPositionPrefab, _finishPosition, Quaternion.Euler(0,0,0)) as GameObject;
            }
        }

        if (Input.GetButton("Fire1"))
        {
            if (transform.position == _finishPosition)
            {
                Destroy(_finishPositionMark);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                _moveLock = true;
                _bulletSpeed = Mathf.Abs(-hit.point.x) + Mathf.Abs(-hit.point.z);
                Debug.Log("Speed = " + Mathf.Abs(-hit.point.x) + Mathf.Abs(-hit.point.z));
                if (Mathf.Abs(-hit.point.x) > 0.1 || Mathf.Abs(-hit.point.z) > 0.1)
                {
                    Vector3 HitPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    transform.LookAt(2 * transform.position - HitPos);
                    arrow.transform.position = HitPos;
                }
                else
                {
                    //transform.rotation = Quaternion.LookRotation(Vector3.back);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _finishPosition, speed);
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            Destroy(_finishPositionMark);
            if (_moveLock == true)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == 8 && hit.point != _finishPosition)
                {
                    _moveLock = false;
                    arrow.transform.position = transform.position;
                    GameObject bulletInstance = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation) as GameObject;
                    Rigidbody instBulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
                    instBulletRigidbody.centerOfMass = new Vector3(0, 0, 0.5f);
                    instBulletRigidbody.AddForce(transform.forward * _bulletSpeed * bulletSpeedMult);
                }
            }
        }
    }


}
