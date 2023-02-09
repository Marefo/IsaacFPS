using _CodeBase.Infrastructure.Services;
using Zenject;

namespace _CodeBase.UI.Buttons
{
  public class RestartButton : ButtonUI
  {
    private SceneService _sceneService;

    [Inject]
    public void Construct(SceneService sceneService)
    {
      _sceneService = sceneService;
    }

    protected override void OnClick() => _sceneService.ReloadCurrentScene();
  }
}