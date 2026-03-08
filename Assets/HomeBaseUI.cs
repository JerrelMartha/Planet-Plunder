using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeBaseUI : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Session");
    }
}
