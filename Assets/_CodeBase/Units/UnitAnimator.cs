using UnityEngine;

namespace _CodeBase.Units
{
  public class UnitAnimator : MonoBehaviour
  {
    [SerializeField] private Animator _animator;

    private int _runHash = Animator.StringToHash("IsRunning");
    private int _attackHash = Animator.StringToHash("Attack");

    public void ChangeRunState(bool enable) => _animator.SetBool(_runHash, enable);
    public void PlayAttack() => _animator.SetTrigger(_attackHash);
  }
}