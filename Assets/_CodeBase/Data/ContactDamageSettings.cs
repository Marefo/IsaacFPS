using UnityEngine;

namespace _CodeBase.Data
{
  [CreateAssetMenu(fileName = "ContactDamageSettings", menuName = "Settings/ContactDamage")]
  public class ContactDamageSettings : ScriptableObject
  {
    public int Damage;
    public float KnockBackForce;
  }
}