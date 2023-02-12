using System;
using UnityEngine;

namespace _CodeBase.Etc
{
  public class GroundChecker : MonoBehaviour
  {
    public event Action<float> Landed;
    
    public bool Grounded { get; private set; }
    
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector3 _groundCheckBoxSize;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask _floorLayer;

    private bool? _lastFrameGroundedValue;

    private void FixedUpdate()
    {
      Grounded = Physics.CheckBox(_groundCheckPoint.position, _groundCheckBoxSize, transform.rotation, _floorLayer);
      
      if(_lastFrameGroundedValue is false && Grounded)
        Landed?.Invoke(_rigidbody.velocity.y);

      _lastFrameGroundedValue = Grounded;
    }
    
    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckBoxSize);
    }
  }
}