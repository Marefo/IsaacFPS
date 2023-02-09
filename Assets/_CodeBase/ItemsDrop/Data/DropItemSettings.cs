using UnityEngine;

namespace _CodeBase.ItemsDrop.Data
{
  [CreateAssetMenu(fileName = "DropItemSettings", menuName = "Settings/DropItem")]
  public class DropItemSettings : ScriptableObject
  {
    public float PushForce;
  }
}