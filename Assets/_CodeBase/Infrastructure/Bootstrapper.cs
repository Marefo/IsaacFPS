using _CodeBase.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace _CodeBase.Infrastructure
{
  public class Bootstrapper : MonoBehaviour
  {
    private const string BootstrapSceneName = "Bootstrap";
    private const string GameSceneName = "Game";
    
    private SceneService _sceneService;

    [Inject]
    private void Construct(SceneService sceneService)
    {
      _sceneService = sceneService;
    }

    private void Awake() => _sceneService.LoadScene(GameSceneName);
  }
}