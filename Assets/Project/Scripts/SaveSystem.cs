using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/playerData.json";

    [System.Serializable]
    public struct SaveData
    {
        public PlayerStats.StatSaveData playerStats;
        public List<NodeSaveData> nodesData;
    }

    public static void SaveGame()
    {
        SaveData data = new SaveData();

        // Save Stats
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.SaveData(ref data.playerStats);
        }

        // Save Nodes via GameManager
        if (GameManager.instance != null)
        {
            data.nodesData = GameManager.instance.GetNodeSaveData();
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"Game Saved to: {savePath}");
    }

    public static void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Load Stats
            if (PlayerStats.instance != null)
            {
                PlayerStats.instance.LoadData(data.playerStats);
            }

            // Load Nodes via GameManager
            if (GameManager.instance != null)
            {
                GameManager.instance.ApplyNodeLoadData(data.nodesData);
            }

            Debug.Log("Game Loaded Successfully");
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }

    [System.Serializable]
    public struct NodeSaveData
    {
        public string id;
        public bool unlocked;
        public int upgradeCount;
    }
}