using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.HeroCode
{
  public class CameraHeadBobPlayer : MonoBehaviour
  {
    [SerializeField] private float _idleSpeed;
    [SerializeField] private float _idleAmount;
    [Space(10)]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _runAmount;
    [Space(10)]
    [SerializeField] private HeroMovement _heroMovement;

    private float _timer;

    private void Update() => HandleHeadBob();

    private void HandleHeadBob()
    {
      bool isHeroMoving = Mathf.Abs(_heroMovement.MoveDirection.x) > 0.1f ||
                          Mathf.Abs(_heroMovement.MoveDirection.z) > 0.1f;
      
      _timer += Time.deltaTime * (isHeroMoving ? _runSpeed : _idleSpeed);
      Vector3 newPosition = transform.position;
      newPosition.y = _heroMovement.transform.position.y + Mathf.Sin(_timer) * (isHeroMoving ? _runAmount : _idleAmount);
      transform.position = newPosition;
    }
  }
}