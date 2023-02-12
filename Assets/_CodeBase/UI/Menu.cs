using _CodeBase.Infrastructure.Services;
using Zenject;

namespace _CodeBase.UI
{
  public class Menu : Screen
  {
    private InputService _inputService;
    private TimeService _timeService;
    private AudioService _audioService;

    [Inject]
    public void Construct(InputService inputService, TimeService timeService, AudioService audioService)
    {
      _inputService = inputService;
      _timeService = timeService;
      _audioService = audioService;
    }

    private void OnEnable() => _inputService.MenuButtonClicked += ChangeVisibility;
    private void OnDisable() => _inputService.MenuButtonClicked -= ChangeVisibility;

    public void ChangeVisibility()
    {
      _audioService.PlaySfx(_audioService.SfxData.BookPageTurn, true);
      
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