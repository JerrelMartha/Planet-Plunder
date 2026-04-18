using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateToScene : MonoBehaviour
{
    public void Navigate(string scene)
    {
        Time.timeScale = 1f;
        SaveSystem.SaveGame();
        SceneManager.LoadScene(scene);
    }
}
