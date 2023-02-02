using System;
using UnityEngine;

namespace _CodeBase.Etc
{
  public class SinMover : MonoBehaviour
  {
    [SerializeField] private float _amount;
    [SerializeField] private float _speed;
    
    private float _defaultPositionY;
    private float _timer;

    private void Start() => _defaultPositionY = transform.localPosition.y;

    private void Update()
    {
      _timer += Time.deltaTime * _speed;
      Vector3 newPosition = transform.localPosition;
      newPosition.y = _defaultPositionY + Mathf.Sin(_timer) * _amount;
      transform.localPosition = newPosition;
    }
  }
}