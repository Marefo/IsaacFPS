using System.Collections;
using _CodeBase.StateMachineCode;
using _CodeBase.Units.GaperCode.Data;
using UnityEngine;

namespace _CodeBase.Units.GaperCode.States
{
  public class AttackState : State
  {
    private readonly Gaper _gaper;
    private readonly GaperSettings _settings;
    private Coroutine _attackCoroutine;

    public AttackState(Gaper gaper, GaperSettings settings)
    {
      _gaper = gaper;
      _settings = settings;
    }

    public override void Enter()
    {
      base.Enter();
      _attackCoroutine = _gaper.StartCoroutine(AttackCoroutine());
    }

    public override void Exit()
    {
      base.Exit();
      _gaper.StopCoroutine(_attackCoroutine);
    }

    private IEnumerator AttackCoroutine()
    {
      while (true)
      {
        _gaper.Attack();
        yield return new WaitForSeconds(_settings.AttackCooldown);
      }
    }
  }
}