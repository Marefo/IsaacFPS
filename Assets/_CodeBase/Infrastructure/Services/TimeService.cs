using System.Collections;
using _CodeBase.Extensions;
using UnityEngine;

namespace _CodeBase.Infrastructure.Services
{
  public class TimeService : MonoBehaviour
  {
    [SerializeField] private float _slowDownMultiplier = 0.2f;
    [SerializeField] private float _slowDownSpeed = 0.25f;
    [SerializeField] private float _normalizeSpeed = 0.25f;

    private void Start() => NormalizeFast();

    public void Stop()
    {
      StopAllCoroutines();
      SetTimeScale(0);
    }

    public void NormalizeFast()
    {
      StopAllCoroutines();
      SetTimeScale(1);
    }

    public void SlowDownFast()
    {
      StopAllCoroutines();
      SetTimeScale(_slowDownMultiplier);
    }
    
    public void SlowDown()
    {
      StopAllCoroutines();
      StartCoroutine(ChangeTimeScaleValueCoroutine(_slowDownMultiplier, _slowDownSpeed));
    }

    public void Normalize()
    {
      StopAllCoroutines();
      StartCoroutine(ChangeTimeScaleValueCoroutine(1, _normalizeSpeed));
    }

    private IEnumerator ChangeTimeScaleValueCoroutine(float targetValue, float speed)
    {
      if (targetValue == Time.timeScale) yield break;

      int sign = Time.timeScale.GetSignForInterpolation(targetValue);
      float minValue = Mathf.Min(Time.timeScale, targetValue);
      float maxValue = Mathf.Max(Time.timeScale, targetValue);

      while (true)
      {
        SetTimeScale(Mathf.Clamp(Time.timeScale + sign * speed * Time.deltaTime, minValue, maxValue));

        if (Time.timeScale == targetValue)
          yield break;
				
        yield return null;
      }
    }

    private void SetTimeScale(float value)
    {
      Time.timeScale = value;
      Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
  }
}