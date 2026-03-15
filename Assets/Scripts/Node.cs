using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [SerializeField] private NodeSO node;

    private Image imageComponent;
    private Transform background;
    private Image backgroundImage;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        background = transform.parent;
        backgroundImage = background.GetComponent<Image>();
    }
    private void Start()
    {
        imageComponent.sprite = node.upgradeIcon;
        UpgradeTypeColorChange(node.upgradeType);
    }

    private void Update()
    {
        if (HasPrerequisites())
        {
            Unlock();
        }
    }

    public bool CanAfford()
    {
        // Gets the CostData in the costs List.
        foreach (var costData in node.costs)
        {
            // From the CostData gets the resource type that needs to be paid and gets the players resource amount.
            float currentAmount = PlayerResources.instance.GetResourceAmount(costData.resourceType);

            // Checks if player has enough of that resource.
            if (currentAmount < costData.cost)
            {
                return false;
            }
            // Loop again if upgrade requires more than one resource.
        }

        // returns true if player has enough resources for the upgrade.
        return true;
    }

    public bool HasPrerequisites()
    {
        // If the list is null or empty, there are no requirements, so return true
        if (node.prerequisites == null || node.prerequisites.Count == 0)
        {
            return true;
        }

        foreach (var node in node.prerequisites)
        {
            // If that upgrade isn't unlocked returns false
            if (!node.isUnlocked)
            {
                return false;
            }
        }
        // All upgrades are unlocked.
        return true;
    }

    public bool IsUnlocked()
    {
        return node.isUnlocked;
    }

    public void Unlock()
    {
        node.isUnlocked = true;
    }

    public void Buy()
    {
        if (!CanBuy()) return;


        foreach (var costData in node.costs)
        {
            PlayerResources.instance.AddResource(costData.resourceType, -costData.cost); // Removes upgrade cost from player inventory
            PlayerStats.instance.IncreaseStat(node.statToUpgrade, node.upgradeAdd); // Upgrades the stat.
            
            if (node.currentUpgradeAmount < node.maxUpgrades)
            {
                node.currentUpgradeAmount++;
            }
            costData.cost += costData.cost; 
        }

        PlayerStats.instance.InitializeAllStats();
    }

    public bool IsMaxedOut()
    {
        return node.currentUpgradeAmount >= node.maxUpgrades;
    }

    public bool CanBuy()
    {
        return !IsMaxedOut() && CanAfford() && HasPrerequisites() && IsUnlocked();
    }

    private void UpgradeTypeColorChange(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Offense:
                backgroundImage.color = Color.red;
                break;
            case UpgradeType.Mobility:
                backgroundImage.color = Color.cyan;
                break;
            case UpgradeType.Utility:
                backgroundImage.color = Color.limeGreen;
                break;
            case UpgradeType.Economy:
                backgroundImage.color = Color.yellow;
                break;
            default:
                break;
        }

    }

}
