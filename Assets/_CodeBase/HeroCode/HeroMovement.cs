using System;
using System.Collections;
using System.Collections.Generic;
using _CodeBase.Extensions;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Logging;
using UnityEngine;
using Zenject;

public class HeroMovement : MonoBehaviour
{
  [SerializeField] private float _moveSpeed;
  [Space(10)] 
  [SerializeField] private Transform _orientation;
  [SerializeField] private Rigidbody _rigidbody;
  
  private InputService _inputService;
  
  [Inject]
  public void Construct(InputService inputService)
  {
    _inputService = inputService;
  }

  private void FixedUpdate() => Move();

  private void Move()
  {
    Vector3 input = _inputService.MovementInput;
    Vector3 direction = _orientation.forward * input.z + _orientation.right * input.x;
    direction.y = 0;
    
    _rigidbody.AddForce(direction.normalized * _moveSpeed, ForceMode.Force);
  }
}