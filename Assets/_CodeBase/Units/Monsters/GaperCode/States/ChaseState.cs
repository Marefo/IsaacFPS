using _CodeBase.Infrastructure;
using _CodeBase.StateMachineCode;
using UnityEngine.AI;

namespace _CodeBase.Units.Monsters.GaperCode.States
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
      _agent.destination = Helpers.RandomPointOnCircleEdge(_stateMachine.Hero.transform.position, 1.5f);
  }
}