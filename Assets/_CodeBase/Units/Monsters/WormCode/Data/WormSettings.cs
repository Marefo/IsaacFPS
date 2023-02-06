using _CodeBase.ShooterCode;
using _CodeBase.ShooterCode.Data;
using UnityEngine;

namespace _CodeBase.Units.Monsters.WormCode.Data
{
  [CreateAssetMenu(fileName = "WormSettings", menuName = "Settings/Monsters/Worm", order = 0)]
  public class WormSettings : ScriptableObject
  {
    public float MoveDistance;
    [Space(10)]
    public float DisappearDelay;
    public float AppearDelay;
    public float AttackDelay;
    [Space(10)]
    public float GroundAppearTime;
    public float GroundDisappearTime;
    [Space(10)] 
    public Bullet BulletPrefab;
    public BulletSettings BulletSettings;
  }
}