using _CodeBase.Infrastructure;
using _CodeBase.Logging;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.Monsters.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.Monsters.CommonStates
{
  public class ChaseState : State
  {
    private readonly ITargetProvider _targetProvider;
    private readonly NavMeshAgent _agent;
    private readonly bool _randomizeDestination;

    public ChaseState(ITargetProvider targetProvider, NavMeshAgent agent, bool randomizeDestination)
    {
      _targetProvider = targetProvider;
      _agent = agent;
      _randomizeDestination = randomizeDestination;
    }

    public override void Exit()
    {
      base.Exit();
      _agent.destination = _targetProvider.GetTarget().position;
    }

    public override void Update() => Chase();

    private void Chase()
    {
      Vector3 destination = _targetProvider.GetTarget().position;

      if (_randomizeDestination)
        destination = Helpers.RandomPointOnCircleEdge(_targetProvider.GetTarget().position, 1.5f);
      
      _agent.destination = destination;
    }
  }
}