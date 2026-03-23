using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public bool autosaveOn = true;
    private float saveTimer = 0f;
    [SerializeField] private float saveFrequency = 15f;

    private void Start()
    {
        LoadGame();
    }

    private void Update()
    {
        AutoSave();

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            Save();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            DeleteSave();
        }


    }

    private void AutoSave()
    {
        if (!autosaveOn) return;

        saveTimer += Time.deltaTime;

        if (saveTimer >= saveFrequency)
        {
            saveTimer = 0f;
            Save();
        }
    }

    [ContextMenu("Save Game")]
    public void Save()
    {
        SaveSystem.SaveGame();
    }

    public void LoadGame()
    {
        SaveSystem.LoadGame();
    }

    public void DeleteSave()
    {
        SaveSystem.DeleteSave();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If this was a real build, you might want to quit the app or reload the menu
        Application.Quit(); 
#endif
    }
}
