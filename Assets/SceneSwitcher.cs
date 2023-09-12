using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Call this function to switch to a new scene by its name
    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Call this function to switch to a new scene by its build index
    public void SwitchToScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
