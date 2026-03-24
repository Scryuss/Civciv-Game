using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public event Action OnPlayerJumped;

    [Header("Player Movement")]
    [SerializeField] float _movementSpeed;

    [Header("Player Orientation")]
    [SerializeField] Transform _orientationTransform;

    [Header("Jump Settings")]
    [SerializeField] KeyCode _jumpKey;
    [SerializeField] float _jumpCooldown;
    [SerializeField] float _jumpForce;
    [SerializeField] float _airMultiplier;
    [SerializeField] float _airDrag;
    [SerializeField] private bool _canJump;

    [Header("Sliding Settings")]
    [SerializeField] float _slideMultiplier;
    [SerializeField] float _slideDrag;

    [Header("Ground Check")]
    [SerializeField] float _playerHeight;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundDrag;

    private float _startingMovementSpeed;
    private float _startingJumpForce;
    private StateController _stateController;
    private Rigidbody _playerRigidbody;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _movementDirection;

    private bool _isSliding;

    

    void Awake()
    {
        _stateController = GetComponent<StateController>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;

        _startingMovementSpeed = _movementSpeed;
        _startingJumpForce = _jumpForce;
    }

    void Update()
    {
        SetInputs();
        SetStates();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }

    void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isSliding = true;
        }
        else
        {
            _isSliding = false;
        }

        if (Input.GetKeyDown(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCooldown);
        }
    }

    private void SetStates()
    {
        var movementDirection = GetMovementDirection();
        var isGrounded = IsGrounded();
        var isSliding = IsSliding();
        var currentState = _stateController.GetCurrentPlayerState();

        var newState = currentState switch
         {
            _ when movementDirection == Vector3.zero && isGrounded && !isSliding => PlayerState.Idle,
            _ when movementDirection != Vector3.zero && isGrounded && !isSliding => PlayerState.Move,
            _ when movementDirection != Vector3.zero && isGrounded && isSliding => PlayerState.Slide,
            _ when movementDirection == Vector3.zero && isGrounded && isSliding => PlayerState.SlideIdle,
            _ when !_canJump && !isGrounded => PlayerState.Jump,
            _ => currentState
         };

         if (newState != currentState)
         {
            _stateController.ChangeState(newState);
         }
    }

    private void SetPlayerMovement()
    {
        // World space'de hareket (kameraya bağlı değil - WASD = mutlak yönler)
        _movementDirection = Vector3.forward * _verticalInput + Vector3.right * _horizontalInput;

        float forceMultiplier = _stateController.GetCurrentPlayerState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => _slideMultiplier,
            PlayerState.Jump => _airMultiplier,
            _ => 1f
        };

        _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed * forceMultiplier, ForceMode.Force);
    }

    private void SetPlayerDrag()
    {
        _playerRigidbody.linearDamping = _stateController.GetCurrentPlayerState() switch
        {
            PlayerState.Slide or PlayerState.SlideIdle => _slideDrag,
            PlayerState.Idle or PlayerState.Move => _groundDrag,
            PlayerState.Jump => _airDrag,
            _ => _playerRigidbody.linearDamping
        };
    }

    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);

        if (flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
            _playerRigidbody.linearVelocity = new Vector3(limitedVelocity.x, _playerRigidbody.linearVelocity.y, limitedVelocity.z);
        }    
    }

    private void SetPlayerJumping()
    {
        OnPlayerJumped?.Invoke();
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJumping()
    {
        _canJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
    }

    private Vector3 GetMovementDirection()
    {
        return _movementDirection.normalized;
    }

    private bool IsSliding()
    {
        return _isSliding;
    }

    public void SetMovementSpeed(float speed, float duration)
    {
        _movementSpeed += speed;
        Invoke(nameof(ResetMovementSpeed), duration);
    }

    private void ResetMovementSpeed()
    {
        _movementSpeed = _startingMovementSpeed;
    }

    public void SetJumpForce(float jumpForce, float duration)
    {
        _jumpForce += jumpForce;
        Invoke(nameof(ResetJumpForce), duration);
    }

    private void ResetJumpForce()
    {
        _jumpForce = _startingJumpForce;
    }

    public Rigidbody GetPlayerRigidbody()
    {
        return _playerRigidbody;
    }
}
