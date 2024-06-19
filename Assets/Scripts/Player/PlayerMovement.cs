using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private float _mouseSensetive;

    [SerializeField]
    private Transform _cameraCenter;

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Transform _modelTransform;
    [SerializeField]
    private GroundChecker _groundChecker;
    [SerializeField]
    Collider _attackCollider;

    private Rigidbody _rb;
    private Vector3 _moveVector;
    private Vector3 _prevMousePosition;
    private float _eulerY;
    private float _eulerX;
    private bool _grounded;
    
    private float _freezeSeconds;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (_freezeSeconds > 0 || Time.deltaTime == 0)
        {
            _freezeSeconds -= Time.deltaTime;
            _prevMousePosition = Input.mousePosition;
            return;
        }

        CameraRotate();
        PlayerMove();

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

        if(Input.GetKeyDown(KeyCode.Mouse0) && _grounded)
        {
            _animator.SetTrigger("Attack");
            _freezeSeconds = 1f;
            _attackCollider.enabled = true;
            StartCoroutine(disableAttackTrigger());
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _groundChecker.Grounded())
        {
            _animator.SetTrigger("Jump");      
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);        
        }

    }

    private void PlayerMove()
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

    private void CameraRotate()
    {
        float deltaX = (Input.mousePosition.x - _prevMousePosition.x) * _mouseSensetive;
        float deltaY = (Input.mousePosition.y - _prevMousePosition.y) * _mouseSensetive * -1;
        _prevMousePosition = Input.mousePosition;
        _eulerY += deltaX;
        _eulerX = Mathf.Clamp(_eulerX + deltaY, -35f, 25f);
        transform.eulerAngles = new Vector3(0, _eulerY, 0);
        _cameraCenter.localEulerAngles = new Vector3(_eulerX, 0f, 0f);
    }

    IEnumerator disableAttackTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        _attackCollider.enabled = false;
    }

}
