using System;
using UnityEngine;

namespace _CodeBase.HeroCode
{
  public class CameraHeadBobPlayer : MonoBehaviour
  {
    [SerializeField] private float _bobSpeed;
    [SerializeField] private float _bobAmount;
    [Space(10)]
    [SerializeField] private HeroMovement _heroMovement;

    private float timer;

    private void Update() => HandleHeadBob();

    private void HandleHeadBob()
    {
      if (Mathf.Abs(_heroMovement.MoveDirection.x) <= 0.1f && Mathf.Abs(_heroMovement.MoveDirection.z) <= 0.1f)
      {
        transform.position =
          new Vector3(transform.position.x, _heroMovement.transform.position.y, transform.position.z);
        return;
      }

      timer += Time.deltaTime * _bobSpeed;

      Vector3 newPosition = transform.position;
      newPosition.y = _heroMovement.transform.position.y + Mathf.Sin(timer) * _bobAmount;
      transform.position = newPosition;
    }
  }
}