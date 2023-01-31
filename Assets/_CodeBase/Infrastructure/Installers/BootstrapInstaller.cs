using _CodeBase.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace _CodeBase.Infrastructure.Installers
{
  public class BootstrapInstaller : MonoInstaller
  {
    [SerializeField] private InputService _inputService;
    
    public override void InstallBindings()
    {
      Container.Bind<InputService>().FromComponentInNewPrefab(_inputService).AsSingle().NonLazy();
    }
  }
}