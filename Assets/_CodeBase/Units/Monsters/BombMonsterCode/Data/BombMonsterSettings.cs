using UnityEngine;

namespace _CodeBase.Units.Monsters.BombMonsterCode.Data
{
  [CreateAssetMenu(fileName = "BombMonsterSettings", menuName = "Settings/Monsters/Bomb")]
  public class BombMonsterSettings : ScriptableObject
  {
    public float ActivationDelay;
    public float MoveSpeed;
    [Space(10)]
    public ParticleSystem DestroyVfx;
  }
}