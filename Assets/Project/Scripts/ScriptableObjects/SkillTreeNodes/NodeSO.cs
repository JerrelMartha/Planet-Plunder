using System.Collections.Generic;
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
    public float baseCost;
    public float costIncrement = 5f;

    public float GetCurrentCost(int currentLevel)
    {
        return baseCost + (currentLevel * costIncrement);
    }

    public bool CanAffordResource(int currentLevel)
    {
        return PlayerResources.instance.GetResourceAmount(resourceType) >= GetCurrentCost(currentLevel);
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
    public bool isVisible;
    public bool isPurchased;
    public bool isMaxedOut;

    public void Reset()
    {
        currentUpgradeAmount = 0;
        isUnlocked = isUnlockedByDefault;
        isVisible = false;
        isPurchased = false;
        isMaxedOut = false;
    }
}