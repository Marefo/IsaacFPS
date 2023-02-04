using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.Monsters.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.Monsters.PooterCode.States
{
  public class EscapeState : State
  {
    private readonly PooterStateMachine _stateMachine;
    private readonly NavMeshAgent _agent;
    private readonly ITargetProvider _targetProvider;

    public EscapeState(PooterStateMachine stateMachine, NavMeshAgent agent, ITargetProvider targetProvider)
    {
      _stateMachine = stateMachine;
      _agent = agent;
      _targetProvider = targetProvider;
    }

    public override void Exit()
    {
      base.Exit();
      _agent.SetDestination(_stateMachine.transform.position);
    }

    public override void Update()
    {
      base.Update();
      Vector3 direction = Vector3.Normalize(_stateMachine.transform.position - _targetProvider.GetTarget().transform.position);
      Vector3 targetPosition = _targetProvider.GetTarget().transform.position + direction * 100;
      targetPosition = targetPosition.GetNavMeshSampledPosition();
      _agent.SetDestination(targetPosition);
    }
  }
}