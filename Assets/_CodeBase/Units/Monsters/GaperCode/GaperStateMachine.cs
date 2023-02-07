using System;
using System.Collections.Generic;
using System.Linq;
using _CodeBase.Etc;
using _CodeBase.HeroCode;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.Monsters.CommonStates;
using _CodeBase.Units.Monsters.GaperCode.Data;
using _CodeBase.Units.Monsters.GaperCode.States;
using _CodeBase.Units.Monsters.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.Monsters.GaperCode
{
  public class GaperStateMachine : MonsterStateMachine, ITargetProvider
  {
    [SerializeField] private TriggerListener _attackZone;
    [SerializeField] private Gaper _gaper;
    [Space(10)]
    [SerializeField] private GaperSettings _settings;
    
    private bool _isHeroInAttackZone;

    protected override void OnEnable()
    {
      base.OnEnable();
      _attackZone.Entered += OnAttackZoneEnter;
      _attackZone.Canceled += OnAttackZoneCancel;
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      _attackZone.Entered -= OnAttackZoneEnter;
      _attackZone.Canceled -= OnAttackZoneCancel;
    }

    private void Start()
    {
      InitializeStates();
      InitializeStateTransitions();
      InitializeStartState();
    }

    public Transform GetTarget() => Hero.transform;

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
        [typeof(ChaseState)] = new ChaseState(this, _agent, true),
        [typeof(AttackState)] = new AttackState(_gaper, _settings),
      };
      
      _stateMachine.AddStates(states);
    }

    private void InitializeStartState()
    {
      Collider heroCollider = _gaper.RoomZone.GetHeroFromZone();

      if (heroCollider != null)
      {
        Hero = heroCollider.GetComponent<Hero>();
        IsHeroInRoomZone = true;
        _stateMachine.EnterState(_stateMachine.GetState<ChaseState>());
      }
      else
        _stateMachine.EnterState(_stateMachine.GetState<IdleState>());
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
      
      At(idleState, chaseState, () => IsHeroInRoomZone);
      At(idleState, attackState, () => _isHeroInAttackZone);
    }

    private void InitializeChaseStateTransitions()
    {
      var chaseState = _stateMachine.GetState<ChaseState>();
      var idleState = _stateMachine.GetState<IdleState>();
      var attackState = _stateMachine.GetState<AttackState>();
      
      At(chaseState, idleState, () => IsHeroInRoomZone == false);
      At(chaseState, attackState, () => _isHeroInAttackZone);
    }

    private void InitializeAttackStateTransitions()
    {
      var attackState = _stateMachine.GetState<AttackState>();
      var chaseState = _stateMachine.GetState<ChaseState>();
      var idleState = _stateMachine.GetState<IdleState>();
      
      At(attackState, chaseState, () => _isHeroInAttackZone == false);
      At(attackState, idleState, () => IsHeroInRoomZone == false);
    }

    private void At(State from, State to, Func<bool> condition) => 
      _stateMachine.AddTransition(from, to, condition);
  }
}