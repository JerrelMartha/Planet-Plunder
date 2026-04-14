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
        public List<PlayerResources.ResourceSaveData> resourcesData;
        public List<string> unlockedWeaponIDs;
    }

    public static void SaveGame()
    {
        SaveData data = new SaveData();

        if (PlayerStats.instance != null) PlayerStats.instance.SaveData(ref data.playerStats);
        if (GameManager.instance != null) data.nodesData = GameManager.instance.GetNodeSaveData();

        if (PlayerResources.instance != null)
        {
            data.resourcesData = PlayerResources.instance.GetSaveData();
        }

        if (WeaponManager.instance != null)
        {
            data.unlockedWeaponIDs = WeaponManager.instance.GetUnlockedWeaponIDs();
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public static void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (PlayerStats.instance != null) PlayerStats.instance.LoadData(data.playerStats);
            if (GameManager.instance != null) GameManager.instance.ApplyNodeLoadData(data.nodesData);

            if (PlayerResources.instance != null)
            {
                PlayerResources.instance.LoadData(data.resourcesData);
            }

            if (WeaponManager.instance != null)
            {
                WeaponManager.instance.LoadData(data.unlockedWeaponIDs);
            }
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
        public bool isVisible;
        public bool isPurchased;
        public bool isMaxedOut;
    }
}