using _CodeBase.StateMachineCode;

namespace _CodeBase.Units.Monsters.MonstroCode.States
{
  public class AttackState : State
  {
    private readonly Monstro _monstro;

    public AttackState(Monstro monstro)
    {
      _monstro = monstro;
    }

    public override void Enter()
    {
      base.Enter();
      _monstro.Attack();
    }
  }
}