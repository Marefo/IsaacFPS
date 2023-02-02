using System;
using System.Collections.Generic;
using _CodeBase.Etc;
using _CodeBase.HeroCode;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.GaperCode.Data;
using _CodeBase.Units.GaperCode.States;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.GaperCode
{
  public class GaperStateMachine : MonoBehaviour
  {
    public Hero Hero { get; private set; }

    [SerializeField] private TriggerListener _roomZone;
    [SerializeField] private TriggerListener _attackZone;
    [SerializeField] private Gaper _gaper;
    [SerializeField] private NavMeshAgent _agent;
    [Space(10)]
    [SerializeField] private GaperSettings _settings;
    
    private StateMachine _stateMachine;
    private bool _isHeroInRoomZone;
    private bool _isHeroInAttackZone;

    private void OnEnable()
    {
      _roomZone.Entered += OnRoomZoneEnter;
      _roomZone.Canceled += OnRoomZoneCancel;
      _attackZone.Entered += OnAttackZoneEnter;
      _attackZone.Canceled += OnAttackZoneCancel;
    }

    private void OnDisable()
    {
      _roomZone.Entered -= OnRoomZoneEnter;
      _roomZone.Canceled -= OnRoomZoneCancel;
      _attackZone.Entered -= OnAttackZoneEnter;
      _attackZone.Canceled -= OnAttackZoneCancel;
    }

    private void Start()
    {
      InitializeStates();
      InitializeStateTransitions();
      _stateMachine.EnterState(_stateMachine.GetState<IdleState>());
    }

    private void Update() => _stateMachine.Update();

    private void FixedUpdate() => _stateMachine.FixedUpdate();

    private void OnDrawGizmos() => _stateMachine?.OnDrawGizmos();

    private void OnRoomZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      Hero = hero;
      _isHeroInRoomZone = true;
    }

    private void OnRoomZoneCancel(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      Hero = null;
      _isHeroInRoomZone = false;
    }

    private void OnAttackZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _isHeroInAttackZone = true;
    }

    private void OnAttackZoneCancel(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _isHeroInAttackZone = false;
    }

    private void InitializeStates()
    {
      _stateMachine = new StateMachine();
      
      Dictionary<Type, State> states = new Dictionary<Type, State>()
      {
        [typeof(IdleState)] = new IdleState(),
        [typeof(ChaseState)] = new ChaseState(this, _agent),
        [typeof(AttackState)] = new AttackState(_gaper, _settings),
      };
      
      _stateMachine.AddStates(states);
    }

    private void InitializeStateTransitions()
    {
      InitializeIdleStateTransitions();
      InitializeChaseStateTransitions();
      InitializeAttackStateTransitions();
    }

    private void InitializeIdleStateTransitions()
    {
      var idleState = _stateMachine.GetState<IdleState>();
      var chaseState = _stateMachine.GetState<ChaseState>();
      var attackState = _stateMachine.GetState<AttackState>();
      
      At(idleState, chaseState, () => _isHeroInRoomZone);
      At(idleState, attackState, () => _isHeroInAttackZone);
    }

    private void InitializeChaseStateTransitions()
    {
      var chaseState = _stateMachine.GetState<ChaseState>();
      var idleState = _stateMachine.GetState<IdleState>();
      var attackState = _stateMachine.GetState<AttackState>();
      
      At(chaseState, idleState, () => _isHeroInRoomZone == false);
      At(chaseState, attackState, () => _isHeroInAttackZone);
    }

    private void InitializeAttackStateTransitions()
    {
      var attackState = _stateMachine.GetState<AttackState>();
      var chaseState = _stateMachine.GetState<ChaseState>();
      var idleState = _stateMachine.GetState<IdleState>();
      
      At(attackState, chaseState, () => _isHeroInAttackZone == false);
      At(attackState, idleState, () => _isHeroInRoomZone == false);
    }

    private void At(State from, State to, Func<bool> condition) => 
      _stateMachine.AddTransition(from, to, condition);
  }
}