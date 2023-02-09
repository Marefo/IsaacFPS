using _CodeBase.Infrastructure.Services;
using _CodeBase.UI;
using UnityEngine;
using Zenject;

namespace _CodeBase.Infrastructure.Installers
{
  public class BootstrapInstaller : MonoInstaller
  {
    [SerializeField] private InputService _inputService;
    [SerializeField] private SceneService _sceneService;
    [SerializeField] private TimeService _timeService;
    [SerializeField] private NavMeshService _navMeshService;
    [SerializeField] private LoadingCurtain _loadingCurtain;
    
    public override void InstallBindings()
    {
      Container.Bind<InputService>().FromComponentInNewPrefab(_inputService).AsSingle().NonLazy();
      Container.Bind<SceneService>().FromComponentInNewPrefab(_sceneService).AsSingle().NonLazy();
      Container.Bind<TimeService>().FromComponentInNewPrefab(_timeService).AsSingle().NonLazy();
      Container.Bind<NavMeshService>().FromComponentInNewPrefab(_navMeshService).AsSingle().NonLazy();
      Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(_loadingCurtain).AsSingle().NonLazy();
    }
  }
}