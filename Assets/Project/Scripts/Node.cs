using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[ExecuteAlways]
public class Node : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private NodeSO node;

    private Image imageComponent;
    private Transform background;
    private Image backgroundImage;

    [SerializeField] private GameObject tooltip;
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI upgradeAmount;
    [SerializeField] private GameObject costUI;

    private bool costSpawned = false;

    // Keeps track of all cost ui spawned
    private List<TextMeshProUGUI> spawnedCostTexts = new List<TextMeshProUGUI>();

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
        transform.parent.name = node.upgradeName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);
        SetupTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    private void Update()
    {
        CheckNodeState();
    }

    public bool CanAfford()
    {
        foreach (var costData in node.costs)
        {
            float currentAmount = PlayerResources.instance.GetResourceAmount(costData.resourceType);
            if (currentAmount < costData.cost) return false;
        }
        return true;
    }

    public bool HasPrerequisites()
    {
        if (node.prerequisites == null || node.prerequisites.Count == 0) return true;

        foreach (var prereq in node.prerequisites)
        {
            // Change from .isUnlocked to .isPurchased 
            // to ensure the player actually BOUGHT the previous tier.
            if (!prereq.isPurchased) return false;
        }
        return true;
    }

    public bool IsUnlocked() => node.isUnlocked;
    public void Unlock()
    {
        node.isUnlocked = true;
    }

    public void Buy()
    {
        if (!CanBuy()) return;

        foreach (var costData in node.costs)
        {
            PlayerResources.instance.AddResource(costData.resourceType, -costData.cost);
        }

        node.isPurchased = true;
        node.isUnlocked = true;
        PlayerStats.instance.IncreaseStat(node.statToUpgrade, node.upgradeAdd);

        if (node.currentUpgradeAmount < node.maxUpgrades)
        {
            node.currentUpgradeAmount++;
        }

        if (IsMaxedOut())
        {
            node.isMaxedOut = true;
        }

        SetupTooltip();
        CheckVisible();
        PlayerStats.instance.InitializeAllStats();
    }
    public bool IsMaxedOut() => node.currentUpgradeAmount >= node.maxUpgrades;

    public bool CanBuy()
    {
        return !IsMaxedOut() && CanAfford() && HasPrerequisites() && IsUnlocked();
    }

    private void UpgradeTypeColorChange(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Offense: backgroundImage.color = Color.red; break;
            case UpgradeType.Mobility: backgroundImage.color = Color.cyan; break;
            case UpgradeType.Utility: backgroundImage.color = Color.green; break;
            case UpgradeType.Economy: backgroundImage.color = Color.yellow; break;
        }
    }

    private void SetupTooltip()
    {
        upgradeName.text = node.upgradeName;
        upgradeDescription.text = node.upgradeDescription;
        upgradeAmount.text = $"{node.currentUpgradeAmount} / {node.maxUpgrades}";

        Vector2[] positions = new Vector2[] { new Vector2(0, -125), new Vector2(-200, -125), new Vector2(200, -125) };

        // Spawns Cost UI once
        if (!costSpawned)
        {
            costSpawned = true;
            for (int i = 0; i < node.costs.Count; i++)
            {
                CreateCostUI(positions[i], node.costs[i]);
            }
        }

        // 2. Always update the colors/values when the tooltip opens
        UpdateAllCostVisuals();
    }

    private void CreateCostUI(Vector2 position, CostData costData)
    {
        GameObject obj = Instantiate(costUI);
        
        Transform tooltipContainer = tooltip.transform.Find("tooltip") ?? tooltip.transform;
        obj.transform.SetParent(tooltipContainer, false);

        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        Image resourceImage = obj.GetComponentInChildren<Image>();
        TextMeshProUGUI resourceValue = obj.GetComponentInChildren<TextMeshProUGUI>();

        rectTransform.anchoredPosition = position;
        resourceImage.sprite = costData.resourceIcon;

        // Adds resource card to list so it can get updated later
        spawnedCostTexts.Add(resourceValue);
    }

    private void UpdateAllCostVisuals()
    {
        for (int i = 0; i < node.costs.Count; i++)
        {
            if (i >= spawnedCostTexts.Count) break;

            CostData costData = node.costs[i];
            TextMeshProUGUI text = spawnedCostTexts[i];

            text.text = costData.cost.ToString();

            // If player can afford one cost item turn white else red
            text.color = costData.CanAffordResource() ? Color.white : Color.red;
        }
    }

    private void CheckNodeState()
    {
        if (HasPrerequisites())
        {
            Unlock();
        }

        CheckVisible();
    }

    private void CheckVisible()
    {
        // If there are no prerequisites, it should always be visible
        if (node.prerequisites == null || node.prerequisites.Count == 0)
        {
            SetNodeVisibility(true);
            return;
        }

        // Check if ALL prerequisites are purchased
        bool allPurchased = true;
        foreach (var prereq in node.prerequisites)
        {
            // Assuming your NodeSO has a 'isPurchased' or similar bool
            if (!prereq.isPurchased)
            {
                allPurchased = false;
                break;
            }
        }

        SetNodeVisibility(allPurchased);

        // Apply a "Locked" visual style if it's visible but not yet purchased
        if (allPurchased && !node.isPurchased)
        {
            imageComponent.color = Color.gray;
        }
        else if (node.isPurchased)
        {
            imageComponent.color = Color.white;
        }
    }

    private void SetNodeVisibility(bool visible)
    {
        // Option A: Use a CanvasGroup (Best for performance/animations)
        // You'll need: private CanvasGroup canvasGroup; in Awake()
        if (TryGetComponent<CanvasGroup>(out CanvasGroup group))
        {
            group.alpha = visible ? 1 : 0;
            group.interactable = visible;
            group.blocksRaycasts = visible;
        }
        else
        {
            // Option B: Simple Toggle (The background and icon)
            imageComponent.enabled = visible;
            if (backgroundImage != null) backgroundImage.enabled = visible;
        }
    }
}