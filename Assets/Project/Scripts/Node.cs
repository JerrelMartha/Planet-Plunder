using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
    private List<TextMeshProUGUI> spawnedCostTexts = new List<TextMeshProUGUI>();


    private void OnValidate()
    {

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(transform.parent.gameObject, "Rename Node Parent");
#endif
        transform.parent.name = node.upgradeName;

        upgradeName.text = node.upgradeName;
        upgradeDescription.text = node.upgradeDescription;
        upgradeAmount.text = $"{node.currentUpgradeAmount} / {node.maxUpgrades}";

        imageComponent = GetComponent<Image>();
        backgroundImage = transform.parent.GetComponent<Image>();

        imageComponent.sprite = node.upgradeIcon;
        UpgradeTypeColorChange(node.upgradeType);
    }

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
        if (HasPrerequisites())
        {
            Unlock();
        }
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
            if (!prereq.isUnlocked) return false;
        }
        return true;
    }

    public bool IsUnlocked() => node.isUnlocked;
    public void Unlock() => node.isUnlocked = true;

    public void Buy()
    {
        if (!CanBuy()) return;

        foreach (var costData in node.costs)
        {
            PlayerResources.instance.AddResource(costData.resourceType, -costData.cost);
        }

        PlayerStats.instance.IncreaseStat(node.statToUpgrade, node.upgradeAdd);

        if (node.currentUpgradeAmount < node.maxUpgrades)
        {
            node.currentUpgradeAmount++;
        }

        SetupTooltip();
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

        if (!costSpawned && Application.isPlaying) // Only spawn objects while the game is running
        {
            costSpawned = true;
            for (int i = 0; i < node.costs.Count; i++)
            {
                CreateCostUI(positions[i], node.costs[i]);
            }
        }

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

        spawnedCostTexts.Add(resourceValue);
    }

    private void UpdateAllCostVisuals()
    {
        // Only attempt this if the game is running and costs are spawned
        if (!Application.isPlaying) return;

        for (int i = 0; i < node.costs.Count; i++)
        {
            if (i >= spawnedCostTexts.Count) break;

            CostData costData = node.costs[i];
            TextMeshProUGUI text = spawnedCostTexts[i];

            text.text = costData.cost.ToString();
            text.color = costData.CanAffordResource() ? Color.white : Color.red;
        }
    }
}