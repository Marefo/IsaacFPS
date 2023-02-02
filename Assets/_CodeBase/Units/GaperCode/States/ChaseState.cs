using _CodeBase.HeroCode;
using _CodeBase.StateMachineCode;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Units.GaperCode.States
{
  public class ChaseState : State
  {
    private readonly GaperStateMachine _stateMachine;
    private float _pushDistance;
    private NavMeshAgent _agent;
    private UnitAnimator _animator;

    public ChaseState(GaperStateMachine stateMachine)
    {
      _stateMachine = stateMachine;
    }

    public override void Exit()
    {
      base.Exit();
      _animator.ChangeRunState(false);
    }

    public override void Update()
    {
      Chase();
      UpdateRunAnimationState();
    }

    private void Chase() => 
      _agent.destination = _stateMachine.Hero.transform.position;

    private void UpdateRunAnimationState() => 
      _animator.ChangeRunState(_agent.remainingDistance != 0);
  }
}