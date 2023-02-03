using System;
using UnityEngine;

namespace _CodeBase.Units.Monsters.PacerCode
{
  public class PacerAnimator : MonoBehaviour
  {
    public event Action LandImpacted;

    public void OnLandImpact() => LandImpacted?.Invoke();
  }
}