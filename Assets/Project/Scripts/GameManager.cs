using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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

        if (Keyboard.current.sKey.wasPressedThisFrame) Save();
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

    // --- Data Bridge for SaveSystem ---

    public List<SaveSystem.NodeSaveData> GetNodeSaveData()
    {
        List<SaveSystem.NodeSaveData> dataList = new List<SaveSystem.NodeSaveData>();
        if (allNodes == null) return dataList;

        foreach (var node in allNodes)
        {
            dataList.Add(new SaveSystem.NodeSaveData
            {
                id = node.nodeID,
                unlocked = node.isUnlocked,
                upgradeCount = node.currentUpgradeAmount
            });
        }
        return dataList;
    }

    public void ApplyNodeLoadData(List<SaveSystem.NodeSaveData> savedData)
    {
        if (savedData == null || allNodes == null) return;

        foreach (var data in savedData)
        {
            // Find the asset that matches the ID in the save file
            foreach (var node in allNodes)
            {
                if (node.nodeID == data.id)
                {
                    node.isUnlocked = data.unlocked;
                    node.currentUpgradeAmount = data.upgradeCount;
                    break;
                }
            }
        }
    }
}