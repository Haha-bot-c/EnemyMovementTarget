using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerMover : MonoBehaviour
{
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";
    private const string CommandJump = "Jump";
    private const string MouseXAxisName = "Mouse X";
    private const string MouseYAxisName = "Mouse Y";
    private const float GroundCheckRadius = 0.1f;
    private const float GroundCheckDistance = -2f;
    private const float GravityMultiplier = -2f;
    private const int RotateMouseButton = 2;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f; 
    private float _rotationSpeed = 2f;
    private bool _isGrounded;
    private Vector3 _velocity;
    private CharacterController _controller;
    private Animator _animator;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        HandleMovement(currentPosition);
        HandleRotation();
        Jump();
    }

    private void HandleMovement(Vector3 position)
    {
        _animator.SetBool("Grounded", _isGrounded);
        Vector3 forwardDirection = transform.forward;
        Vector3 rightDirection = transform.right;
        forwardDirection.y = 0f;
        rightDirection.y = 0f;
        forwardDirection.Normalize();
        rightDirection.Normalize();
        float horizontalInput = Input.GetAxis(HorizontalInput);
        float verticalInput = Input.GetAxis(VerticalInput);
        Vector3 move = rightDirection * horizontalInput + forwardDirection * verticalInput;
        _controller.Move(move * _speed * Time.deltaTime);
        _animator.SetFloat("MoveSpeed", verticalInput);
    }


    private void HandleRotation()
    {
        if (Input.GetMouseButton(RotateMouseButton))
        {
            float mouseX = Input.GetAxis(MouseXAxisName);
            float mouseY = Input.GetAxis(MouseYAxisName);
            transform.Rotate(Vector3.up, mouseX * _rotationSpeed, Space.World);
            transform.Rotate(Vector3.left, mouseY * _rotationSpeed, Space.Self);
        }
    }

    private void Jump()
    {
        _isGrounded = Physics.CheckSphere(transform.position, GroundCheckRadius, _groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = GroundCheckDistance;
        }

        if (Input.GetButtonDown(CommandJump) && _isGrounded)
        {
            _isGrounded = false;
            _velocity.y = Mathf.Sqrt(_jumpHeight * GravityMultiplier * _gravity);
        }
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
