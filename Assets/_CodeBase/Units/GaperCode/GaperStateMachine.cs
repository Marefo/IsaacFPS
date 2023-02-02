using System;
using System.Collections.Generic;
using _CodeBase.Etc;
using _CodeBase.HeroCode;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.GaperCode.Data;
using _CodeBase.Units.GaperCode.States;
using UnityEngine;

namespace _CodeBase.Units.GaperCode
{
  public class GaperStateMachine : MonoBehaviour
  {
    public Hero Hero { get; private set; }

    [SerializeField] private Gaper _gaper;
    [SerializeField] private TriggerListener _roomZone;
    [Space(10)]
    [SerializeField] private GaperSettings _settings;
    
    private StateMachine _stateMachine;
    private bool _isHeroInRoomZone;

    private void OnEnable()
    {
      _roomZone.Entered += OnRoomZoneEnter;
      _roomZone.Canceled += OnRoomZoneCancel;
    }

    private void OnDisable()
    {
      _roomZone.Entered -= OnRoomZoneEnter;
      _roomZone.Canceled -= OnRoomZoneCancel;
    }

    private void Start()
    {
      InitializeStates();
      InitializeStateTransitions();
      _stateMachine.EnterState(_stateMachine.GetState<IdleState>());
    }

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

    private void InitializeStates()
    {
      _stateMachine = new StateMachine();
      
      Dictionary<Type, State> states = new Dictionary<Type, State>()
      {
        [typeof(IdleState)] = new IdleState(),
        [typeof(ChaseState)] = new ChaseState(this),
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
      At(idleState, attackState, CanAttack);
    }

    private void InitializeChaseStateTransitions()
    {
      var chaseState = _stateMachine.GetState<ChaseState>();
      var idleState = _stateMachine.GetState<IdleState>();
      var attackState = _stateMachine.GetState<AttackState>();
      
      At(chaseState, idleState, () => _isHeroInRoomZone == false);
      At(chaseState, attackState, CanAttack);
    }

    private void InitializeAttackStateTransitions()
    {
      var attackState = _stateMachine.GetState<AttackState>();
      var chaseState = _stateMachine.GetState<ChaseState>();
      var idleState = _stateMachine.GetState<IdleState>();
      
      At(attackState, chaseState, () => CanAttack() == false);
      At(attackState, idleState, () => _isHeroInRoomZone == false);
    }

    private void At(State from, State to, Func<bool> condition) => 
      _stateMachine.AddTransition(from, to, condition);

    private bool CanAttack() =>
      Hero != null && Vector3.Distance(transform.position, Hero.transform.position) <= _settings.DistanceForAttack;
  }
}