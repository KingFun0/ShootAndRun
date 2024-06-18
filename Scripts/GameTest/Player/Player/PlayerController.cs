using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PhotonView _photonView;
    static public string _playerTag;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _cameraSensitivity = 2f;
    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _checkJumpradius = 0.2f;
    [SerializeField] private float _jumpForce = 3f;
    [SerializeField] private Animator _animator;
    private float _rotationX;
    private bool _isCrouching = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        if (_photonView.IsMine)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (!_photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(_rigidbody);
        }
    }

    private void FixedUpdate()
    {
        if (_photonView.IsMine)
        {
            PlayerMovement();
            SetPlayerTag(_playerTag);
        }
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            RotatePlayerRightLeft();
            RotateCameraUpDown();
            Clutch();
        }
    }

    public void SetPlayerTag(string tag)
    {
        _playerTag = tag;
        gameObject.tag = _playerTag;
    }

    private void Clutch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            _isCrouching = !_isCrouching;
            _animator.SetBool("isCrouch", _isCrouching);
        }
    }

    private void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movementDir = transform.forward * v + transform.right * h;
        float speed = movementDir.magnitude * _movementSpeed;

        _rigidbody.velocity = new Vector3(movementDir.x * _movementSpeed, _rigidbody.velocity.y, movementDir.z * _movementSpeed);

        if (speed >= 0.5f)
        {
            _animator.SetBool("isRun", true);
            _animator.SetBool("isCrouch", false);
        }
        else
        {
            _animator.SetBool("isRun", false);
        }
    }

    private void RotatePlayerRightLeft()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _cameraSensitivity);
    }

    private void RotateCameraUpDown()
    {
        _rotationX -= _cameraSensitivity * Input.GetAxis("Mouse Y");
        _rotationX = Mathf.Clamp(_rotationX, -75, 75);
        _camera.eulerAngles = new Vector3(_rotationX, _camera.eulerAngles.y, _camera.eulerAngles.z);
    }
}
