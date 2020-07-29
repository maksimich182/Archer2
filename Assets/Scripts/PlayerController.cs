using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject finishPositionPrefab;
    public GameObject shootVectorPrefab;
    public GameObject negativeMessagePrefab;
    public Material shootVectorMaterial;
    public Transform gunPoint;
    public int bulletSpeedMult = 10;
    public float speed = 5f;
    public float multSzShootVector = 5f;
    public float lifeTimeNegativeMessage = 3f;

    private bool _isPullingBow = false;
    private bool _animationPullingStringLock = false;
    private bool _createShootVector = false;
    private bool _isNegativeMessage = false;
    private float _bulletSpeed;
    private float _timeNegativeMessage = 0f;
    private Vector3 _finishPosition;
    private GameObject _finishPositionMark;
    private GameObject _shootVector;
    private GameObject _negativeMessage;
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
                if (_rayMousePosition.collider.gameObject.layer == 8 || _isPullingBow == true)
                {
                    _isPullingBow = true;
                    if (_animationPullingStringLock == false)
                    {
                        _animationPullingStringLock = true;
                        _animation["PullingString"].speed = 2f;
                        _animation.Play("PullingString", PlayMode.StopAll);
                    }

                    PullBowString(_rayMousePosition);
                }
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
            _createShootVector = false;
            Destroy(_shootVector);
            _animation.Play("Idle_Bow", PlayMode.StopAll);
            Destroy(_finishPositionMark);
            if (_isPullingBow == true)
            {
                GetMousePosition(out _rayMousePosition);
                bool isXZCoordinateEquation = _rayMousePosition.point.x == _finishPosition.x && _rayMousePosition.point.z == _finishPosition.z;
                if (isXZCoordinateEquation == false)
                {
                    _isPullingBow = false;
                    _animation.Play("Shoot", PlayMode.StopAll);
                    Shoot();
                }
                else
                {
                    _animation["PullingString"].speed = -2f;
                    _animation["PullingString"].time = _animation["PullingString"].length;
                    _animation.Play("PullingString", PlayMode.StopAll);
                }
            }
            
        }

        if (_negativeMessage == true)
        {
            if (_timeNegativeMessage > lifeTimeNegativeMessage)
            {
                _isNegativeMessage = false;
                _timeNegativeMessage = 0f;
                Destroy(_negativeMessage);
            }
            else
            {
                _timeNegativeMessage += Time.deltaTime;
                _negativeMessage.transform.position = this.transform.position;
            }
        }
    }

    private void Shoot()
    {
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
        _bulletSpeed = Vector3.Distance(new Vector3(hit.point.x, 0f, hit.point.z), _finishPosition);
        Vector3 HitPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        transform.LookAt(2 * transform.position - HitPos);
        DrawShootVector();
    }

    private void DrawShootVector()
    {
        if (_createShootVector == false)
        {
            _createShootVector = true;
            _shootVector = Instantiate(shootVectorPrefab, transform.position, transform.rotation) as GameObject;
        }
        else
        {
            float valueMinPowerColor = 1f * multSzShootVector;
            float valueMidPowerColor = 2f * multSzShootVector;
            float valueMaxPowerColor = 3f * multSzShootVector;

            if (multSzShootVector * _bulletSpeed < valueMaxPowerColor)
            {
                _shootVector.transform.localScale = new Vector3(1, 1, multSzShootVector * _bulletSpeed);
            }
            _shootVector.transform.rotation = transform.rotation;
            if (multSzShootVector * _bulletSpeed < valueMinPowerColor)
            {
                shootVectorMaterial.color = Color.green;
            }
            else if (multSzShootVector * _bulletSpeed < valueMidPowerColor)
            {
                float colorMix = ((multSzShootVector * _bulletSpeed) - valueMinPowerColor) / (valueMidPowerColor - valueMinPowerColor);
                shootVectorMaterial.color = Color.Lerp(Color.green, Color.yellow, colorMix);
            }
            else if (multSzShootVector * _bulletSpeed < valueMaxPowerColor)
            {
                float colorMix = ((multSzShootVector * _bulletSpeed) - valueMidPowerColor) / (valueMaxPowerColor - valueMidPowerColor);
                shootVectorMaterial.color = Color.Lerp(Color.yellow, Color.red, colorMix);
            }
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
            _isPullingBow = false;
            _finishPosition = _rayMousePosition.point;
            _finishPositionMark = Instantiate(finishPositionPrefab, _finishPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
            _finishPosition.y = 0f;     //TODO: Костыльная методика задачи координат. Исправить
        }
        else
        {
            _finishPosition = this.transform.position;
            if (_isNegativeMessage == false)
            {
                _isNegativeMessage = true;
                _negativeMessage = Instantiate(negativeMessagePrefab, this.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            }
        }
    }
}
