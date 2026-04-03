using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private GameObject optionsScreen;
    private bool optionsActive = false;
    public void StartGame()
    {
        SceneManager.LoadScene("Session");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void OpenOptions()
    {
        optionsActive = !optionsActive;
        optionsScreen.SetActive(optionsActive);
    }
}
