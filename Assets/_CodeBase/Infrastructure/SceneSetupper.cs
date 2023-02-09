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
    private NavMeshService _navMeshService;
    
    [Inject]
    public void Construct(InputService inputService, TimeService timeService, NavMeshService navMeshService)
    {
      _inputService = inputService;
      _timeService = timeService;
      _navMeshService = navMeshService;
    }

    private void Awake()
    {
      _timeService.NormalizeFast();
      _inputService.Enable();
      _navMeshService.ReBake();
    }
  }
}