using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateToScene : MonoBehaviour
{
    public void Navigate(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
