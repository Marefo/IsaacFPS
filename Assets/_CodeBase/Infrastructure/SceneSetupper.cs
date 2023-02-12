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
    private AudioService _audioService;
    
    [Inject]
    public void Construct(InputService inputService, TimeService timeService, NavMeshService navMeshService, AudioService audioService)
    {
      _inputService = inputService;
      _timeService = timeService;
      _navMeshService = navMeshService;
      _audioService = audioService;
    }

    private void Awake()
    {
      _timeService.NormalizeFast();
      _inputService.Enable();
      _navMeshService.ReBake();
      _audioService.ChangeMusicTo(_audioService.MusicData.Main);
    }
  }
}