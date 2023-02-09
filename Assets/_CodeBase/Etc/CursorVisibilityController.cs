using UnityEngine;

namespace _CodeBase.Etc
{
  public static class CursorVisibilityController
  {
    public static void Show()
    {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }

    public static void Hide()
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
  }
}