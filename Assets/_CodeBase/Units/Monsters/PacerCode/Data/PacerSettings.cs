using _CodeBase.Data;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.Units.Monsters.PacerCode.Data
{
  [CreateAssetMenu(fileName = "PacerSettings", menuName = "Settings/Monsters/Pacer", order = 0)]
  public class PacerSettings : ScriptableObject
  {
    public float ResetRotationSpeed;
    [Space(10)]
    public float JumpCooldown;
    public Range JumpDistance;
    public float JumpHeight;
    public float JumpDuration;
    public Ease JumpEase;
  }
}