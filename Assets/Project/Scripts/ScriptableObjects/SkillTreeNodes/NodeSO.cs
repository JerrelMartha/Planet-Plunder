using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public enum UpgradeType
{
    Offense,
    Mobility,
    Utility,
    Economy,
}

[System.Serializable]
public class CostData
{
    public Resource resourceType;
    public Sprite resourceIcon;
    public float cost;

    public bool CanAffordResource()
    {
        return cost < PlayerResources.instance.GetResourceAmount(resourceType);
    }
}

[CreateAssetMenu(fileName = "NewNode", menuName = "Scriptable Objects/NodeSO")]
public class NodeSO : ScriptableObject
{
    public string nodeID;
    [Header("UI Info")]
    public string upgradeName;
    [TextArea] public string upgradeDescription;
    public Sprite upgradeIcon;

    [Header("Upgrade Logic")]
    public UpgradeType upgradeType;
    public Stats statToUpgrade;
    public int currentUpgradeAmount;
    public int maxUpgrades;
    public float upgradeAdd;

    [Header("Progression")]
    public List<NodeSO> prerequisites;
    public List<CostData> costs;
    public bool isUnlocked;
    public bool isUnlockedByDefault;

    public void Reset()
    {
        currentUpgradeAmount = 0;
        isUnlocked = isUnlockedByDefault;
    }
}
