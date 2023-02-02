using _CodeBase.HeroCode;
using _CodeBase.StateMachineCode;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.GaperCode.States
{
  public class ChaseState : State
  {
    private readonly GaperStateMachine _stateMachine;
    private readonly NavMeshAgent _agent;

    public ChaseState(GaperStateMachine stateMachine, NavMeshAgent agent)
    {
      _stateMachine = stateMachine;
      _agent = agent;
    }

    public override void Exit()
    {
      base.Exit();
      _agent.destination = _stateMachine.transform.position;
    }

    public override void Update() => Chase();

    private void Chase() => 
      _agent.destination = _stateMachine.Hero.transform.position;
  }
}