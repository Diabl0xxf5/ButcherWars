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
    private Animator _animator;
    [SerializeField]
    private Transform _modelTransform;
    [SerializeField]
    private GroundChecker _groundChecker;
    [SerializeField]
    Collider _attackCollider;

    private Rigidbody _rb;
    private Vector3 _moveVector;
    private float _prevMousePositionX;
    private float _eulerY;
    private bool _grounded;
    
    private float _freezeSeconds;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (_freezeSeconds > 0)
        {
            _freezeSeconds -= Time.deltaTime;
            _prevMousePositionX = Input.mousePosition.x;
            return;
        }

        _moveVector.x = Input.GetAxis("Horizontal");
        _moveVector.z = Input.GetAxis("Vertical");

        _animator.SetFloat("Speed", _moveVector.sqrMagnitude);

        if (_moveVector.sqrMagnitude > 0)
        {
            Vector3 direction = transform.TransformDirection(_moveVector);
            direction.Normalize();

            Vector3 modelRoration = Vector3.zero;

            if (_moveVector.x > 0) {
                modelRoration = new Vector3(0, _moveVector.z > 0 ? 45f : _moveVector.z < 0 ? 135f : 90f, 0);
            } else if (_moveVector.x < 0) {
                modelRoration = new Vector3(0, _moveVector.z > 0 ? -45f : _moveVector.z < 0 ? -135f : -90f, 0);
            } else {
                modelRoration = new Vector3(0, _moveVector.z > 0 ? 0 : -180f, 0);
            }

            _modelTransform.localRotation = Quaternion.Lerp(_modelTransform.localRotation, Quaternion.Euler(modelRoration), Time.deltaTime * _rotationSpeed);

            _rb.velocity = new Vector3(direction.x * _speed, _rb.velocity.y, direction.z * _speed);

        } else
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            _modelTransform.localRotation = Quaternion.Lerp(_modelTransform.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * _rotationSpeed);
        }


        //Camera rotate
        float deltaX = (Input.mousePosition.x - _prevMousePositionX) * _mouseSensetive;
        _prevMousePositionX = Input.mousePosition.x;
        _eulerY += deltaX;
        transform.eulerAngles = new Vector3(0, _eulerY, 0);

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

        if(Input.GetMouseButtonDown(0) && _grounded)
        {
            _animator.SetTrigger("Attack");
            _freezeSeconds = 1f;
            _attackCollider.enabled = true;
            StartCoroutine(disableAffterTime());
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _groundChecker.Grounded())
        {
            _animator.SetTrigger("Jump");      
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);        
        }

    }

    IEnumerator disableAffterTime()
    {
        yield return new WaitForSeconds(0.5f);
        _attackCollider.enabled = false;
    }

}
