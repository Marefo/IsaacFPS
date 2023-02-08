using System;
using UnityEngine;

namespace _CodeBase.HeroCode
{
  public class HandsAnimator : MonoBehaviour
  {
    public event Action ThrowFramePlayed;
    public event Action GrenadePickedUp;

    public void PickUpGrenade() => GrenadePickedUp?.Invoke();
    public void OnThrowFrame() => ThrowFramePlayed?.Invoke();
  }
}