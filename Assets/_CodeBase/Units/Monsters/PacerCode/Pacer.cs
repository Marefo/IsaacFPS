using System;
using System.Collections;
using System.Linq;
using _CodeBase.Etc;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.Logging;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Range = _CodeBase.Data.Range;

namespace _CodeBase.Units.Monsters.PacerCode
{
  public class Pacer : Monster
  {
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private Range _jumpDistance;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private Ease _ease;
    [Space(10)]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Transform _landCheckPoint;
    [SerializeField] private float _landCheckSphereRadius;
    [SerializeField] private float _groundCheckSphereRadius;
    [SerializeField] private LayerMask _floorLayer;
    [Space(10)] 
    [SerializeField] private ParticleSystem _landVfx;
    [Space(10)] 
    [SerializeField] private PacerAnimator _pacerAnimator;
    [SerializeField] private UnitAnimator _animator;

    private float _defaultPositionY;
    private bool _grounded = true;
    private bool _canLand = true;
    private bool _landing = true;

    private void OnEnable() => _pacerAnimator.LandImpacted += OnLandImpact;
    private void OnDisable() => _pacerAnimator.LandImpacted -= OnLandImpact;

    private void Start()
    {
      _defaultPositionY = transform.position.y;
      RandomPositionJump();
    }

    private void Update()
    {
      //transform.LookAt(_heroTransform);
    }

    private void FixedUpdate()
    {
      _canLand = Physics.CheckSphere(_landCheckPoint.position, _landCheckSphereRadius, _floorLayer);
      _grounded = Physics.CheckSphere(_groundCheckPoint.position, _groundCheckSphereRadius, _floorLayer);
      
      if(_canLand && _grounded == false)
        Land();
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(_groundCheckPoint.position, _landCheckSphereRadius);
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckSphereRadius);
    }

    private void Jump()
    {
      
    }
    
    private void RandomPositionJump()
    {
      float distance = _jumpDistance.GetRandomValue();
      Vector3 randomDirection = Random.insideUnitSphere * distance;
      randomDirection += transform.position;
      NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, distance, 1);
      Vector3 targetPosition = hit.position;

      _animator.PlayJump();
      transform.DOKill();
      transform.DOJump(targetPosition, _jumpHeight, 1, _jumpDuration)
        .SetEase(_ease);
    }

    private void Land()
    {
      MyDebug.Log($"Land", MyDebug.DebugColor.yellow);
      _landing = true;
      _animator.PlayLand();
    }

    private void OnLandImpact()
    {
      SpawnLandVfx();
      DOVirtual.DelayedCall(_jumpCooldown, RandomPositionJump);
    }

    private void SpawnLandVfx()
    {
      Vector3 spawnPosition = transform.position;
      spawnPosition.y = _defaultPositionY;
      Instantiate(_landVfx, spawnPosition, Quaternion.identity);
    }
  }
}