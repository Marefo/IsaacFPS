using _CodeBase.Infrastructure.Services;
using Zenject;

namespace _CodeBase.UI
{
  public class WinnerLetter : Screen
  {
    private InputService _inputService;

    [Inject]
    public void Construct(InputService inputService)
    {
      _inputService = inputService;
    }

    public override void Open()
    {
      _inputService.Disable();
      base.Open();
    }

    public override void Close()
    {
      _inputService.Enable();
      base.Close();
    }
  }
}