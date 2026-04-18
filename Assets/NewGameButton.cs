using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    public void NewGame()
    {
        SaveSystem.DeleteSave();
        SaveSystem.SaveGame();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Title");
    }
}
