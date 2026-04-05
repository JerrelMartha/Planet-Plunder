using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static SaveSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool autosaveOn = true;
    private float saveTimer = 0f;
    [SerializeField] private float saveFrequency = 15f;

    // Internal array to hold all upgrades found in Resources
    private NodeSO[] allNodes;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        // Automatically find every NodeSO inside Assets/Resources/nodes
        allNodes = Resources.LoadAll<NodeSO>("nodes");
    }

    private void Start()
    {
        LoadGame();
    }

    private void Update()
    {
        AutoSave();

        if (Keyboard.current.pKey.wasPressedThisFrame) Save();
        if (Keyboard.current.qKey.wasPressedThisFrame) DeleteSave();
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
        Debug.Log("Saved Game");
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
        Application.Quit(); 
#endif
        foreach (var node in allNodes)
        {
            node.Reset();
        }
    }


    public List<NodeSaveData> GetNodeSaveData()
    {
        if (allNodes == null) return new List<NodeSaveData>();

        return allNodes.Select(node => new NodeSaveData
        {
            id = node.nodeID,
            unlocked = node.isUnlocked,
            upgradeCount = node.currentUpgradeAmount,
            isVisible = node.isVisible,
            isPurchased = node.isPurchased,
            isMaxedOut = node.isMaxedOut,
        }).ToList();
    }

    public void ApplyNodeLoadData(List<NodeSaveData> savedData)
    {
        if (savedData == null || allNodes == null) return;

        var nodeLookup = allNodes.ToDictionary(n => n.nodeID, n => n);

        foreach (var data in savedData)
        {
            if (nodeLookup.TryGetValue(data.id, out var node))
            {
                node.isUnlocked = data.unlocked;
                node.currentUpgradeAmount = data.upgradeCount;
                node.isVisible = data.isVisible;
                node.isPurchased = data.isPurchased;
                node.isMaxedOut = data.isMaxedOut;
            }
        }
    }

    private void BeforeNavigate()
    {
        Time.timeScale = 1f;
        PlayerResources.instance.ResetTemporaryInventory();
    }
    public void NavigateToUpgrade()
    {
        BeforeNavigate();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("HomeBase");
    }

    public void NavigateToExplore()
    {
        BeforeNavigate();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Explore");
    }

    public void NavigateToTitle()
    {
        BeforeNavigate();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Title");
    }
}