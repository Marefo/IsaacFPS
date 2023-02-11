using _CodeBase.Infrastructure.Services;
using Zenject;

namespace _CodeBase.UI
{
  public class Menu : Screen
  {
    private InputService _inputService;
    private TimeService _timeService;

    [Inject]
    public void Construct(InputService inputService, TimeService timeService)
    {
      _inputService = inputService;
      _timeService = timeService;
    }

    private void OnEnable() => _inputService.MenuButtonClicked += ChangeVisibility;
    private void OnDisable() => _inputService.MenuButtonClicked -= ChangeVisibility;

    public void ChangeVisibility()
    {
      if(IsVisible)
        Close();
      else
        Open();
    }

    public override void Open()
    {
      _inputService.Disable();
      _timeService.Stop();
      base.Open();
    }

    public override void Close()
    {
      _inputService.Enable();
      base.Close();
      _timeService.NormalizeFast();
    }
  }
}