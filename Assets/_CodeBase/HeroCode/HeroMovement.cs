using _CodeBase.Infrastructure.Services;
using _CodeBase.Units;
using UnityEngine;
using Zenject;

public class HeroMovement : MonoBehaviour
{
  public Vector3 MoveDirection => GetMoveDirection();

  [SerializeField] private float _moveSpeed;
  [SerializeField] private float _gravity;
  [SerializeField] private Transform _groundCheckPoint;
  [SerializeField] private float _groundCheckSphereRadius;
  [SerializeField] private LayerMask _floorLayer;
  [Space(10)] 
  [SerializeField] private UnitAnimator _animator;
  [SerializeField] private Transform _orientation;
  [SerializeField] private Rigidbody _rigidbody;
  
  private InputService _inputService;
  
  [Inject]
  public void Construct(InputService inputService)
  {
    _inputService = inputService;
  }

  private void FixedUpdate()
  {
    Move();
    ApplyGravity();
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckSphereRadius);
  }

  private void ApplyGravity()
  {
    bool grounded = Physics.CheckSphere(_groundCheckPoint.position, _groundCheckSphereRadius, _floorLayer);
    if(grounded) return;
    _rigidbody.AddForce(Vector3.down * _gravity);
  }

  private Vector3 GetMoveDirection()
  {
    Vector3 direction = _orientation.forward * _inputService.MovementInput.z + _orientation.right * _inputService.MovementInput.x;
    direction.y = 0;
    return direction;
  }
  
  private void Move()
  {
    _rigidbody.AddForce(MoveDirection.normalized * _moveSpeed, ForceMode.Force);
    _animator.ChangeRunState(MoveDirection != Vector3.zero);
  }
}