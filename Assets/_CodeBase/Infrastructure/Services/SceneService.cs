using UnityEngine;
using UnityEngine.SceneManagement;

namespace _CodeBase.Infrastructure.Services
{
  public class SceneService : MonoBehaviour
  {
    public void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void LoadNextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
  }
}