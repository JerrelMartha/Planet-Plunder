using UnityEngine;

public class CameraSingleton : MonoBehaviour
{
    public static CameraSingleton Instance { get; private set; }

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy it
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the singleton instance
        Instance = this;

        // Prevent the camera from being destroyed when loading a new scene
        DontDestroyOnLoad(gameObject);
    }
}