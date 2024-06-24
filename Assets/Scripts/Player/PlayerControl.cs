using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    public Transform _cameraCenter;
    public Animator _animator;
    public Transform _modelTransform;
    public GroundChecker _groundChecker;
    public Rigidbody _rb;
    public Attack _attack;
    public Grappling _grappling;

    [Header("Settings")]
    public float _rotationSpeed;
    public float _speed;
    public float _jumpForce;
    public float _mouseSensetive;
    public int _damage;
    public int _pushForce;
    public float _freezeControl;

    [Header("Input")]
    public KeyCode _AttackKey;
    public KeyCode _GrappleKey;
    public KeyCode _JumpKey;

    private Vector3 _moveVector;
    private Vector3 _prevMousePosition;
    private float _eulerY;
    private float _eulerX;
    private bool _grounded;
    private float _freezeControlTimer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (_freezeControlTimer > 0 || Time.deltaTime == 0)
        {
            _freezeControlTimer -= Time.deltaTime;
            _prevMousePosition = Input.mousePosition;
            return;
        }

        CameraRotate();
        GroundCheck();

        StateMove();
        StateAttack();
        StateGrappling();
        StateJump();

    }

    private void StateJump()
    {
        if (Input.GetKeyDown(_JumpKey) && _groundChecker.Grounded())
        {
            _animator.SetTrigger("Jump");
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void StateGrappling()
    {
        if (Input.GetKeyDown(_GrappleKey))
        {
            ModelLookAtCamera();
            _grappling.LaunchGrapple();
        }
    }

    private void StateAttack()
    {
        if (Input.GetKeyDown(_AttackKey) && _grounded)
        {
            ModelLookAtCamera();
            _animator.SetTrigger("Attack");
            _freezeControlTimer = _freezeControl;
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            _attack.StartAttack(_damage, _pushForce);
        }
    }
  
    private void StateMove()
    {
        _moveVector.x = Input.GetAxis("Horizontal");
        _moveVector.z = Input.GetAxis("Vertical");

        _animator.SetFloat("Speed", _moveVector.sqrMagnitude);

        if (_moveVector.sqrMagnitude > 0)
        {
            Vector3 direction = transform.TransformDirection(_moveVector);
            direction.Normalize();

            Vector3 modelRoration = Vector3.zero;

            if (_moveVector.x > 0)
            {
                modelRoration = new Vector3(0, _moveVector.z > 0 ? 45f : _moveVector.z < 0 ? 135f : 90f, 0);
            }
            else if (_moveVector.x < 0)
            {
                modelRoration = new Vector3(0, _moveVector.z > 0 ? -45f : _moveVector.z < 0 ? -135f : -90f, 0);
            }
            else
            {
                modelRoration = new Vector3(0, _moveVector.z > 0 ? 0 : -180f, 0);
            }

            _modelTransform.localRotation = Quaternion.Lerp(_modelTransform.localRotation, Quaternion.Euler(modelRoration), Time.deltaTime * _rotationSpeed);

            _rb.velocity = new Vector3(direction.x * _speed, _rb.velocity.y, direction.z * _speed);

        }
        else
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            _modelTransform.localRotation = Quaternion.Lerp(_modelTransform.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * _rotationSpeed);
        }
    }


    private void ModelLookAtCamera()
    {
        Vector3 camera_direction = _cameraCenter.forward;
        camera_direction.y = 0;
        camera_direction.Normalize();

        _modelTransform.LookAt(_modelTransform.position + camera_direction);
    }
    private void CameraRotate()
    {
        float deltaX = (Input.mousePosition.x - _prevMousePosition.x) * _mouseSensetive;
        float deltaY = (_prevMousePosition.y - Input.mousePosition.y) * _mouseSensetive;
        _prevMousePosition = Input.mousePosition;
        _eulerY += deltaX;
        _eulerX = Mathf.Clamp(_eulerX + deltaY, -35f, 25f);
        transform.eulerAngles = new Vector3(0, _eulerY, 0);
        _cameraCenter.localEulerAngles = new Vector3(_eulerX, 0f, 0f);
    }
    private void GroundCheck()
    {
        if (_grounded != _groundChecker.Grounded())
        {
            if (!_grounded)
            {
                _animator.SetTrigger("Landing");
                _grounded = true;
            }
            else
            {
                _animator.SetTrigger("Flying");
                _grounded = false;
            }
        }
    }

}
