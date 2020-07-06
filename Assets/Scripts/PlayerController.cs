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
    private bool _animationPullingStringLock = false;
    private float _bulletSpeed;
    private Vector3 _finishPosition;
    private GameObject _finishPositionMark;
    private Animation _animation;
    private RaycastHit _rayMousePosition;

    private void Start()
    {
        _animation = GetComponent<Animation>();
        _animation["PullingString"].speed = 2f;
        _animation["Shoot"].speed = 2f;
        _animation["Walk_Bow"].speed = 2f;
    }

    void Update()
    {
        if (_animationPullingStringLock == false)
        {
            _animation.PlayQueued("Idle_Bow");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            GetCursorFinishPosition();
        }

        if (Input.GetButton("Fire1"))
        {
            if (transform.position == _finishPosition)
            {
                Destroy(_finishPositionMark);
                GetMousePosition(out _rayMousePosition);
                _moveLock = true;
                if(_animationPullingStringLock == false)
                {
                    _animationPullingStringLock = true;
                    _animation.Play("PullingString", PlayMode.StopAll);
                }
                PullBowString(_rayMousePosition);
            }
            else
            {
                _animation.Play("Walk_Bow", PlayMode.StopAll);
                MoveToPosition();
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            _animationPullingStringLock = false;
            _animation.Play("Idle_Bow", PlayMode.StopAll);
            Destroy(_finishPositionMark);
            if (_moveLock == true)
            {
                GetMousePosition(out _rayMousePosition);
                bool xzCoordinateEquation = _rayMousePosition.point.x == _finishPosition.x && _rayMousePosition.point.z == _finishPosition.z;
                if (_rayMousePosition.collider.gameObject.layer == 8 && xzCoordinateEquation == false)
                {
                    _moveLock = false;
                    _animation.Play("Shoot", PlayMode.StopAll);
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
    }

    private void GetMousePosition(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
    }

    private void PullBowString(RaycastHit hit)
    {
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
            _finishPositionMark = Instantiate(finishPositionPrefab, _finishPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
            _finishPosition.y = 0f;     //TODO: Костыльная методика задачи координат. Исправить
        }
    }
}
