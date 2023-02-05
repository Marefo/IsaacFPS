using System;
using System.Collections.Generic;
using System.Linq;
using _CodeBase.HeroCode;
using _CodeBase.Logging;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.Monsters.CommonStates;
using _CodeBase.Units.Monsters.GaperCode.States;
using _CodeBase.Units.Monsters.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.Monsters.FlyCode
{
  public class FlyStateMachine : MonsterStateMachine, ITargetProvider
  {
    private void Start()
    {
      InitializeStates();
      InitializeStateTransitions();
      InitializeStartState();
    }

    public Transform GetTarget() => Hero.transform;

    private void InitializeStates()
    {
      _stateMachine = new StateMachine();
      
      Dictionary<Type, State> states = new Dictionary<Type, State>()
      {
        [typeof(IdleState)] = new IdleState(),
        [typeof(ChaseState)] = new ChaseState(this, _agent, false),
      };
      
      _stateMachine.AddStates(states);
    }
    
    private void InitializeStartState()
    {
      Collider heroCollider = _monster.RoomZone.CollidersInZone.FirstOrDefault(other => other.GetComponent<Hero>() != null);

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
    }
    
    private void At(State from, State to, Func<bool> condition) => 
      _stateMachine.AddTransition(from, to, condition);
    
    private void InitializeIdleStateTransitions()
    {
      var idleState = _stateMachine.GetState<IdleState>();
      var chaseState = _stateMachine.GetState<ChaseState>();
      
      At(idleState, chaseState, () => IsHeroInRoomZone);
    }

    private void InitializeChaseStateTransitions()
    {
      var chaseState = _stateMachine.GetState<ChaseState>();
      var idleState = _stateMachine.GetState<IdleState>();
      
      At(chaseState, idleState, () => IsHeroInRoomZone == false);
    }
  }
}