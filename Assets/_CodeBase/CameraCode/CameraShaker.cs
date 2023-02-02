using System.Collections;
using _CodeBase.CameraCode.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.CameraCode
{
  public class CameraShaker : MonoBehaviour
  {
    private Vector3 _defaultPosition;
    private float _currentShakeDuration;
    private Coroutine _shakeCoroutine;

    private void Start() => _defaultPosition = transform.localPosition;

    [Button]
    public void Shake(ShakeSettings settings)
    {
      if(_shakeCoroutine != null)
        StopCoroutine(_shakeCoroutine);

      _currentShakeDuration = settings.Duration;
      _shakeCoroutine = StartCoroutine(ShakeCoroutine(settings));
    }

    private IEnumerator ShakeCoroutine(ShakeSettings settings)
    {
      while (true)
      {
        if (_currentShakeDuration > 0)
        {
          Vector3 targetPosition = _defaultPosition + Random.insideUnitSphere * settings.Amount;
          targetPosition.z = _defaultPosition.z;
          transform.localPosition = targetPosition;
          _currentShakeDuration -= Time.deltaTime * settings.DecreaseFactor;
          yield return null;
        }
        else
        {
          _currentShakeDuration = 0f;
          transform.localPosition = _defaultPosition;
          yield break;
        }
      }
    }
  }
}