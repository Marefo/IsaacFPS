using UnityEngine;

namespace _CodeBase.RoomCode.Data
{
  [CreateAssetMenu(fileName = "RoomSettings", menuName = "Settings/Room", order = 0)]
  public class RoomSettings : ScriptableObject
  {
    public float SpawnDelay;
    public float SpawnAfterSmokeDelay;
  }
}