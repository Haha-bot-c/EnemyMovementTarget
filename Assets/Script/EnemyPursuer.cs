using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class EnemyPursuer : MonoBehaviour
{
    private const string MoveSpeed = "MoveSpeed";
    private const float _stoppingDistance = 2f;
    private const float _rotationSpeed = 5f;
    private const float _smoothTime = 0.1f;
    private const float StoppingDistanceSqr = _stoppingDistance * _stoppingDistance;

    [SerializeField] private PlayerMover _target;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _jumpForce = 3f;
    [SerializeField] private float _pushForce = 3f;

    private Animator _animator;
    private Rigidbody _rigidBody;
    private Vector3 _currentVelocity;
    private Vector3 _move;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            _rigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        MoveTowardsTarget();
        LookAtTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        _move = Vector3.SmoothDamp(_rigidBody.velocity, direction * _speed, ref _currentVelocity, _smoothTime);
        float distanceSqr = (transform.position - _target.transform.position).sqrMagnitude;

        if (distanceSqr > StoppingDistanceSqr)
        {
            _rigidBody.velocity = _move;
        }

        _animator.SetFloat(MoveSpeed, _move.magnitude);
    }

    private void LookAtTarget()
    {
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }
}
