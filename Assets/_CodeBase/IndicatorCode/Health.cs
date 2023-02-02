using System;
using UnityEngine;

namespace _CodeBase.IndicatorCode
{
  public class Health : Indicator
  {
    public event Action<int> HealthAmountChanged;
    
    public override void ChangeToZero()
    {
      base.ChangeToZero();
      HealthAmountChanged?.Invoke(CurrentValue);
    }

    protected override void Initialize()
    {
      base.Initialize();
      HealthAmountChanged?.Invoke(CurrentValue);
    }

    protected override void ChangeValue(int value)
    {
      base.ChangeValue(value);
      HealthAmountChanged?.Invoke(CurrentValue);
    }
  }
}