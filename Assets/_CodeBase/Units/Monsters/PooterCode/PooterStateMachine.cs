using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _CodeBase.HeroCode;
using _CodeBase.ShooterCode.Data;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.Monsters.CommonStates;
using _CodeBase.Units.Monsters.Interfaces;
using _CodeBase.Units.Monsters.PooterCode.Data;
using _CodeBase.Units.Monsters.PooterCode.States;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.Units.Monsters.PooterCode
{
  public class PooterStateMachine : MonsterStateMachine, ITargetProvider
  {
    [SerializeField] private Transform _model;
    [SerializeField] private Transform _shootPoint;
    [Space(10)]
    [SerializeField] private UnitAnimator _animator;
    [Space(10)] 
    [SerializeField] private BulletSettings _bulletSettings;
    [SerializeField] private PooterSettings _settings;
    
    private bool _canAttack;
    private bool _attackFinished;
    
    private void Start()
    {
      InitializeStates();
      InitializeStateTransitions();
      InitializeStartState();

      StartCoroutine(AttackCoroutine());
    }

    public Transform GetTarget() => Hero.transform;
    
    public void ChangeAttackPossibilityState(bool enable) => _canAttack = enable;

    public void ChangeAttackState(bool enable) => _attackFinished = enable;
    
    private void InitializeStates()
    {
      _stateMachine = new StateMachine();
      
      Dictionary<Type, State> states = new Dictionary<Type, State>()
      {
        [typeof(IdleState)] = new IdleState(),
        [typeof(AttackState)] = new AttackState(this, _animator, _model, this, _shootPoint, _bulletSettings, _settings),
        [typeof(EscapeState)] = new EscapeState(this, _agent, this),
      };
      
      _stateMachine.AddStates(states);
    }

    private void InitializeStartState()
    {
      Collider heroCollider = _monster.RoomZone.CollidersInZone.FirstOrDefault(other => other.GetComponent<Hero>() != null);

      if(heroCollider != null)
        Hero = heroCollider.GetComponent<Hero>();

      if (heroCollider != null && HasToEscape())
        _stateMachine.EnterState(_stateMachine.GetState<EscapeState>());
      else
        _stateMachine.EnterState(_stateMachine.GetState<IdleState>());
    }

    private void InitializeStateTransitions()
    {
      InitializeIdleStateTransitions();
      InitializeAttackStateTransitions();
      InitializeEscapeStateTransitions();
    }

    private void At(State from, State to, Func<bool> condition) => 
      _stateMachine.AddTransition(from, to, condition);

    private void InitializeIdleStateTransitions()
    {
      var idleState = _stateMachine.GetState<IdleState>();
      var attackState = _stateMachine.GetState<AttackState>();
      var escapeState = _stateMachine.GetState<EscapeState>();
      
      At(idleState, attackState, () => IsHeroInRoomZone && _canAttack && HasToEscape() == false);
      At(idleState, escapeState, () => IsHeroInRoomZone && HasToEscape());
    }

    private void InitializeAttackStateTransitions()
    {
      var attackState = _stateMachine.GetState<AttackState>();
      var escapeState = _stateMachine.GetState<EscapeState>();
      var idleState = _stateMachine.GetState<IdleState>();
      
      At(attackState, idleState, () => IsHeroInRoomZone == false || _attackFinished && HasToEscape() == false);
      At(attackState, escapeState, () => IsHeroInRoomZone && HasToEscape() && _attackFinished);
    }

    private void InitializeEscapeStateTransitions()
    {
      var escapeState = _stateMachine.GetState<EscapeState>();
      var attackState = _stateMachine.GetState<AttackState>();
      var idleState = _stateMachine.GetState<IdleState>();
      
      At(escapeState, attackState, () => IsHeroInRoomZone && _canAttack && HasToEscape() == false);
      At(escapeState, idleState, () => IsHeroInRoomZone == false || 
                                       GetDistanceToHero() >= _settings.EscapeDistance * 1.5f);
    }

    private IEnumerator AttackCoroutine()
    {
      while (true)
      {
        yield return new WaitForSeconds(_settings.AttackDelay);
        ChangeAttackPossibilityState(true);
      }
    }

    private bool HasToEscape() => GetDistanceToHero() <= _settings.EscapeDistance;

    private float GetDistanceToHero() => Vector3.Distance(Hero.transform.position, transform.position);
  }
}