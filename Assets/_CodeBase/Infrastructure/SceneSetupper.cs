using System;
using _CodeBase.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace _CodeBase.Infrastructure
{
  public class SceneSetupper : MonoBehaviour
  {
    private InputService _inputService;
    private TimeService _timeService;
    
    [Inject]
    public void Construct(InputService inputService, TimeService timeService)
    {
      _inputService = inputService;
      _timeService = timeService;
    }

    private void Start()
    {
      _timeService.NormalizeFast();
      _inputService.Enable();
    }
  }
}