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
    private Animator animator;
    private RaycastHit _rayMousePosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GetCursorFinishPosition();
        }

        if (Input.GetButton("Fire1"))
        {
            if (transform.position == _finishPosition)
            {
                animator.SetBool("Walk", false);
                Destroy(_finishPositionMark);
                GetMousePosition(out _rayMousePosition);
                _moveLock = true;
                PullBowString(_rayMousePosition);
            }
            else
            {
                animator.SetBool("Walk", true);
                MoveToPosition();
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("Walk", false);
            Destroy(_finishPositionMark);
            if (_moveLock == true)
            {
                GetMousePosition(out _rayMousePosition);
                if (_rayMousePosition.collider.gameObject.layer == 8 && _rayMousePosition.point != _finishPosition)
                {
                    _moveLock = false;
                    Shoot();
                }
            }
        }
    }

    private void Shoot()
    {
        arrow.transform.position = transform.position;
        GameObject bulletInstance = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation) as GameObject;
        Rigidbody instBulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
        instBulletRigidbody.centerOfMass = new Vector3(0, 0, 0.5f);
        instBulletRigidbody.AddForce(transform.forward * _bulletSpeed * bulletSpeedMult);
        animator.SetBool("Attack", false);
    }

    private void GetMousePosition(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
    }

    private void PullBowString(RaycastHit hit)
    {
        animator.SetBool("Attack", true);
        _bulletSpeed = Vector3.Distance(hit.point, _finishPosition);
        if (Mathf.Abs(-hit.point.x) > 0.1 || Mathf.Abs(-hit.point.z) > 0.1)
        {
            Vector3 HitPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(2 * transform.position - HitPos);
            arrow.transform.position = HitPos;
        }
    }

    private void MoveToPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, _finishPosition, speed);
        transform.LookAt(_finishPosition);
    }

    private void GetCursorFinishPosition()
    {
        GetMousePosition(out _rayMousePosition);
        if (_rayMousePosition.collider.gameObject.layer == 8)
        {
            _finishPosition = _rayMousePosition.point;
            _finishPosition.y = 0f;
            _finishPositionMark = Instantiate(finishPositionPrefab, _finishPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
    }
}
