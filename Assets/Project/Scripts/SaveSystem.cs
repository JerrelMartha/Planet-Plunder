using UnityEngine;
using System.IO;
using System.Collections.Generic;

// Save System made with help from. https://youtu.be/1mf730eb5Wo?si=UCyavIuFoHUvgARU
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

        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.SaveData(ref data.playerStats);
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

            if (PlayerStats.instance != null)
            {
                PlayerStats.instance.LoadData(data.playerStats);
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
        public string id;       // The unique nodeID from the SO
        public bool unlocked;   // Is it currently available?
        public int upgradeCount;       // Current upgrade count
    }
}