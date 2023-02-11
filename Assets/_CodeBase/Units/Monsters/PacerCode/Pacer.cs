using _CodeBase.Etc;
using _CodeBase.Extensions;
using _CodeBase.Infrastructure;
using _CodeBase.Logging;
using _CodeBase.Units.Monsters.PacerCode.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Range = _CodeBase.Data.Range;

namespace _CodeBase.Units.Monsters.PacerCode
{
  public class Pacer : Monster
  {
    [SerializeField] private Transform _model;
    [SerializeField] private TriggerListener _damageZone;
    [Space(10)]
    [SerializeField] private Transform _deathVfxSpawnPoint;
    [SerializeField] private ParticleSystem _deathVfx;
    [SerializeField] private GameObject _deathTrail;
    [SerializeField] private GameObject _jumpBloodTrail;
    [Space(10)]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Transform _landCheckPoint;
    [SerializeField] private Vector3 _landCheckSize;
    [SerializeField] private float _groundCheckSphereRadius;
    [SerializeField] private LayerMask _floorLayer;
    [Space(10)] 
    [SerializeField] private Transform _landVfxSpawnPoint;
    [SerializeField] private ParticleSystem _landVfx;
    [Space(10)] 
    [SerializeField] private PacerAnimator _pacerAnimator;
    [SerializeField] private UnitAnimator _animator;
    [Space(10)] 
    [SerializeField] private PacerSettings _settings;

    private Collider _roomZoneCollider;
    private float _defaultPositionY;
    private Vector3 _targetPosition;
    private Quaternion _modelStartRotation;
    private float _startJumpTime;
    private bool _grounded = true;
    private bool _canLand = true;
    private bool _isJumping;

    private void OnEnable()
    {
      SubscribeEvents();
      Initialized += OnInitialize;
      _pacerAnimator.Jumped += OnJumpFrame;
      _pacerAnimator.LandImpacted += OnLandImpact;
    }

    private void OnDisable()
    {
      UnSubscribeEvents();
      Initialized -= OnInitialize;
      _pacerAnimator.Jumped -= OnJumpFrame;
      _pacerAnimator.LandImpacted -= OnLandImpact;
    }

    private void Start()
    {
      _defaultPositionY = transform.position.y;
      _modelStartRotation = new Quaternion(_model.localRotation.x, _model.localRotation.y, _model.localRotation.z,
        _model.localRotation.w);
      RandomPositionJump();
    }

    private void Update() => HandleModelRotation();

    private void FixedUpdate()
    {
      _canLand = Physics.CheckBox(_landCheckPoint.position, _landCheckSize, Quaternion.identity, _floorLayer);
      _grounded = Physics.CheckSphere(_groundCheckPoint.position, _groundCheckSphereRadius, _floorLayer);
      
      if(Time.time > _startJumpTime + _settings.JumpDuration / 2 && _canLand && _grounded == false)
        Land();
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(_landCheckPoint.position, _landCheckSize);
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckSphereRadius);
    }

    private void OnInitialize() => _roomZoneCollider = RoomZone.GetComponent<Collider>();

    private void OnJumpFrame()
    {
      transform.DOKill();
      transform.DOJump(_targetPosition,  _settings.JumpHeight, 1, _settings.JumpDuration)
        .SetEase(_settings.JumpEase).SetLink(gameObject);
    }

    private void RandomPositionJump()
    {
      _targetPosition = Helpers.GetRandomPositionInCollider(_roomZoneCollider, transform.position.y).GetNavMeshSampledPosition();
      _targetPosition.y = _defaultPositionY;
      
      _isJumping = true;
      _animator.PlayJump();
      _startJumpTime = Time.time;
    }

    private void Land()
    {
      _isJumping = false;
      _startJumpTime = float.MaxValue;
      _animator.PlayLand();
      _damageZone.gameObject.SetActive(true);
    }

    private void OnLandImpact()
    {
      SpawnLandVfx();
      Instantiate(_jumpBloodTrail, transform.position, Quaternion.identity);
      DOVirtual.DelayedCall(_settings.JumpCooldown / 2, () => _damageZone.gameObject.SetActive(false)).SetLink(gameObject);
      DOVirtual.DelayedCall(_settings.JumpCooldown, RandomPositionJump).SetLink(gameObject);
    }

    private void SpawnLandVfx()
    {
      Vector3 spawnPosition = transform.position;
      spawnPosition.y = _defaultPositionY;
      Instantiate(_landVfx, _landVfxSpawnPoint.position, Quaternion.identity);
    }

    private void HandleModelRotation()
    {
      if (_isJumping == false) return;

      if (Time.time <= _startJumpTime + _settings.JumpDuration / 2)
      {
        Vector3 rotationTargetPosition = _targetPosition;
        rotationTargetPosition.y = transform.position.y;
        _model.LookAt(rotationTargetPosition);
      }
      else
      {
        Quaternion target = new Quaternion(_modelStartRotation.x, _model.localRotation.y, _modelStartRotation.z,
          _model.localRotation.w);
        _model.localRotation =
          Quaternion.Slerp(_model.localRotation, target, _settings.ResetRotationSpeed * Time.deltaTime);
      }
    }
    
    protected override void Die()
    {
      Instantiate(_deathTrail, _deathVfxSpawnPoint.position, Quaternion.identity);
      ParticleSystem vfx = Instantiate(_deathVfx, _deathVfxSpawnPoint.position, Quaternion.identity);
      vfx.transform.localScale = Vector3.one * 0.8f;
      base.Die();
      Destroy(gameObject);
    }
  }
}