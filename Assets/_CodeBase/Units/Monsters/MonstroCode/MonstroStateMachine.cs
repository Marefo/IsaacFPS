using System;
using System.Collections;
using System.Collections.Generic;
using _CodeBase.Etc;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.Logging;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.Monsters.CommonStates;
using _CodeBase.Units.Monsters.GaperCode;
using _CodeBase.Units.Monsters.GaperCode.Data;
using _CodeBase.Units.Monsters.Interfaces;
using _CodeBase.Units.Monsters.MonstroCode.Data;
using _CodeBase.Units.Monsters.MonstroCode.States;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.Units.Monsters.MonstroCode
{
  public class MonstroStateMachine : MonsterStateMachine, ITargetProvider
  {
    [SerializeField] private TriggerListener _chaseStopZone;
    [SerializeField] private Monstro _monstro;
    [SerializeField] private MonstroAnimator _monstroAnimator;
    [Space(10)]
    [SerializeField] private MonstroSettings _settings;
    
    private bool _isHeroInAttackZone;
    private bool _attacking;
    private bool _isHeroInChaseStopZone;

    protected override void OnEnable()
    {
      base.OnEnable();
      _monstroAnimator.AttackFinished += OnAttackFinish;
      _chaseStopZone.Entered += OnChaseStopZoneEnter;
      _chaseStopZone.Canceled += OnChaseStopZoneCancel;
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      _monstroAnimator.AttackFinished -= OnAttackFinish;
      _chaseStopZone.Entered -= OnChaseStopZoneEnter;
      _chaseStopZone.Canceled -= OnChaseStopZoneCancel;
    }

    private void Start()
    {
      InitializeStates();
      InitializeStateTransitions();
      InitializeStartState();

      StartCoroutine(AttackCoroutine());
    }

    public Transform GetTarget() => Hero.transform;

    private void OnAttackFinish() => _attacking = false;

    private void OnChaseStopZoneEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _isHeroInChaseStopZone = true;
    }

    private void OnChaseStopZoneCancel(Collider obj)
    {
      if(obj.TryGetComponent(out Hero hero) == false) return;
      _isHeroInChaseStopZone = false;
    }

    private void InitializeStates()
    {
      _stateMachine = new StateMachine();
      
      Dictionary<Type, State> states = new Dictionary<Type, State>()
      {
        [typeof(IdleState)] = new IdleState(),
        [typeof(ChaseState)] = new ChaseState(this, _agent, false),
        [typeof(AttackState)] = new AttackState(_monstro),
      };
      
      _stateMachine.AddStates(states);
    }

    private void InitializeStartState()
    {
      Collider heroCollider = _monster.RoomZone.GetHeroFromZone();

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
      
      At(idleState, chaseState, () => _isHeroInChaseStopZone == false && IsHeroInRoomZone);
    }

    private void InitializeChaseStateTransitions()
    {
      var chaseState = _stateMachine.GetState<ChaseState>();
      var idleState = _stateMachine.GetState<IdleState>();
      var attackState = _stateMachine.GetState<AttackState>();
      
      At(chaseState, idleState, () => _isHeroInChaseStopZone || IsHeroInRoomZone == false);
      At(chaseState, attackState, () => _attacking && IsHeroInRoomZone);
    }

    private void InitializeAttackStateTransitions()
    {
      var attackState = _stateMachine.GetState<AttackState>();
      var chaseState = _stateMachine.GetState<ChaseState>();
      var idleState = _stateMachine.GetState<IdleState>();
      
      At(attackState, idleState, () => IsHeroInRoomZone == false);
      At(attackState, chaseState, () => _attacking == false && IsHeroInRoomZone);
    }

    private void At(State from, State to, Func<bool> condition) => 
      _stateMachine.AddTransition(from, to, condition);

    private IEnumerator AttackCoroutine()
    {
      while (true)
      {
        yield return new WaitForSeconds(_settings.AttackDelay.GetRandomValue());

        if (Hero != null && _attacking == false)
        {
          _attacking = true;
        }
      }
    }
  }
}