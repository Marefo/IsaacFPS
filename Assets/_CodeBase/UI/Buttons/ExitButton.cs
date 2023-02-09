using UnityEngine.Device;

namespace _CodeBase.UI.Buttons
{
  public class ExitButton : ButtonUI
  {
    protected override void OnClick() => Application.Quit();
  }
}