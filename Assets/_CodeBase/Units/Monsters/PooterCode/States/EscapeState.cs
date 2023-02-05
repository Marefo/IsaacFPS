using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.Infrastructure;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.Monsters.Interfaces;
using _CodeBase.Units.Monsters.PooterCode.Data;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.Monsters.PooterCode.States
{
  public class EscapeState : State
  {
    private readonly PooterStateMachine _stateMachine;
    private readonly NavMeshAgent _agent;
    private readonly ITargetProvider _targetProvider;
    private readonly PooterSettings _settings;
    private Vector3 _targetPosition;

    public EscapeState(PooterStateMachine stateMachine, NavMeshAgent agent, ITargetProvider targetProvider, PooterSettings settings)
    {
      _stateMachine = stateMachine;
      _agent = agent;
      _targetProvider = targetProvider;
      _settings = settings;
    }

    public override void Enter()
    {
      base.Enter();
      InitializeTargetPosition();
    }

    public override void Exit()
    {
      base.Exit();
      _agent.SetDestination(_stateMachine.transform.position);
    }

    public override void Update()
    {
      base.Update();

      if (IsTargetPositionFarEnough() == false)
      {
        Vector3 unSampledTargetPosition = Random.insideUnitSphere * _settings.EscapeDistance + _agent.transform.position;
        _targetPosition = unSampledTargetPosition.GetNavMeshSampledPosition();
      }

      _agent.SetDestination(_targetPosition);
    }

    private void InitializeTargetPosition()
    {
      Vector3 direction =
        Vector3.Normalize(_stateMachine.transform.position - _targetProvider.GetTarget().transform.position);
      _targetPosition = _targetProvider.GetTarget().transform.position + direction * 100;
      _targetPosition = _targetPosition.GetNavMeshSampledPosition();
    }

    private bool IsTargetPositionFarEnough() => 
      Vector3.Distance(_targetProvider.GetTarget().transform.position, _targetPosition) >= _settings.EscapeDistance;
  }
}