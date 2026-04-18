using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Node : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private NodeSO node;

    [SerializeField] private Sprite visibleSprite;
    [SerializeField] private Sprite maxedOutSprite;
    [SerializeField] private Sprite offenseSprite;
    [SerializeField] private Sprite mobilitySprite;
    [SerializeField] private Sprite utilitySprite;
    [SerializeField] private Sprite economySprite;

    private Image imageComponent;
    private Transform background;
    private Image backgroundImage;

    [SerializeField] private GameObject tooltip;
    [SerializeField] private GameObject statTooltip;
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI upgradeAmount;
    [SerializeField] private TextMeshProUGUI StatName;
    [SerializeField] private TextMeshProUGUI StatDisplay;
    [SerializeField] private GameObject costUI;

    private bool costSpawned = false;
    private List<TextMeshProUGUI> spawnedCostTexts = new List<TextMeshProUGUI>();

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (node != null && transform.parent != null)
        {
            UnityEditor.Undo.RecordObject(transform.parent.gameObject, "Rename Node Parent");
            transform.parent.name = node.upgradeName;
        }
#endif
        if (node != null)
        {
            upgradeName.text = node.upgradeName;
            upgradeDescription.text = node.upgradeDescription;
            upgradeAmount.text = $"{node.currentUpgradeAmount} / {node.maxUpgrades}";
            imageComponent = GetComponent<Image>();
            if (transform.parent != null) backgroundImage = transform.parent.GetComponent<Image>();
            imageComponent.sprite = node.upgradeIcon;
            UpdateBackgroundVisuals();
        }
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
        UpdateBackgroundVisuals();
        UpdateNodeStatus();
    }

    private void Update()
    {
        UpdateNodeStatus();
    }

    private void UpdateNodeStatus()
    {
        bool prerequisitesMet = HasPrerequisites();

        if (imageComponent.enabled != prerequisitesMet)
        {
            imageComponent.enabled = prerequisitesMet;
        }

        if (backgroundImage != null && backgroundImage.enabled != prerequisitesMet)
        {
            backgroundImage.enabled = prerequisitesMet;
        }

        if (prerequisitesMet)
        {
            UpdateBackgroundVisuals();

            if (node.currentUpgradeAmount > 0)
            {
                imageComponent.color = Color.white;
            }
            else
            {
                imageComponent.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }
    }

    private void UpdateBackgroundVisuals()
    {
        if (backgroundImage == null) return;

        if (IsMaxedOut() && maxedOutSprite != null)
        {
            backgroundImage.sprite = maxedOutSprite;
            backgroundImage.color = Color.white;
        }
        else
        {
            SetBackgroundByType(node.upgradeType);

            if (node.currentUpgradeAmount == 0)
            {
                backgroundImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            else
            {
                backgroundImage.color = Color.white;
            }
        }
    }

    private void SetBackgroundByType(UpgradeType upgradeType)
    {
        Sprite targetSprite = visibleSprite;

        switch (upgradeType)
        {
            case UpgradeType.Offense: targetSprite = offenseSprite; break;
            case UpgradeType.Mobility: targetSprite = mobilitySprite; break;
            case UpgradeType.Utility: targetSprite = utilitySprite; break;
            case UpgradeType.Economy: targetSprite = economySprite; break;
        }

        if (targetSprite != null)
        {
            backgroundImage.sprite = targetSprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HasPrerequisites()) return;
        tooltip.SetActive(true);
        SetupTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    public bool CanAfford()
    {
        foreach (var costData in node.costs)
        {
            if (PlayerResources.instance.GetResourceAmount(costData.resourceType) < costData.GetCurrentCost(node.currentUpgradeAmount))
                return false;
        }
        return true;
    }

    public bool HasPrerequisites()
    {
        if (node.prerequisites == null || node.prerequisites.Count == 0) return true;
        foreach (var prereq in node.prerequisites)
        {
            if (prereq.currentUpgradeAmount <= 0) return false;
        }
        return true;
    }

    public void Buy()
    {
        if (!CanBuy()) return;

        foreach (var costData in node.costs)
        {
            float finalCost = costData.GetCurrentCost(node.currentUpgradeAmount);
            PlayerResources.instance.AddResource(costData.resourceType, -finalCost);
        }

        if (node is WeaponNodeSO weaponNode)
        {
            WeaponManager.instance.UnlockWeapon(weaponNode.weaponID);
        }
        else
        {
            PlayerStats.instance.IncreaseStat(node.statToUpgrade, node.upgradeAdd);
        }

        if (node.currentUpgradeAmount < node.maxUpgrades)
        {
            node.currentUpgradeAmount++;
        }

        if (!IsMaxedOut())
        {
            SoundManager.Instance.PlaySound("NodeUpgrade", true);
        } else
        {
            SoundManager.Instance.PlaySound("NodeMax");
        }
        node.isUnlocked = true;
        SaveSystem.SaveGame();
        SetupTooltip();
        UpdateBackgroundVisuals();
    }

    public bool IsMaxedOut() => node.currentUpgradeAmount >= node.maxUpgrades;

    public bool CanBuy()
    {
        return !IsMaxedOut() && CanAfford() && HasPrerequisites();
    }

    private void SetupTooltip()
    {
        upgradeName.text = node.upgradeName;
        upgradeDescription.text = node.upgradeDescription;
        upgradeAmount.text = $"{node.currentUpgradeAmount} / {node.maxUpgrades}";

        if (node is WeaponNodeSO || IsMaxedOut())
        {
            statTooltip.SetActive(false);
        }
        else
        {
            statTooltip.SetActive(true);
            StatName.text = node.statToUpgrade.ToString();

            float currentStatValue = PlayerStats.instance.GetStatValue(node.statToUpgrade);
            float nextStatValue = currentStatValue + node.upgradeAdd;

            string currentFormatted = currentStatValue.ToString("F1");
            string nextFormatted = nextStatValue.ToString("F1");

            StatDisplay.text = $"{currentFormatted} > <color=#00FF00>{nextFormatted}</color>";
        }

        Vector2[] positions = new Vector2[] { new Vector2(0, -100), new Vector2(-200, -100), new Vector2(200, -100) };

        if (!costSpawned && Application.isPlaying)
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
        if (!Application.isPlaying) return;

        bool isMaxed = IsMaxedOut();

        for (int i = 0; i < node.costs.Count; i++)
        {
            if (i >= spawnedCostTexts.Count) break;

            if (isMaxed)
            {
                spawnedCostTexts[i].text = "MAX";
                spawnedCostTexts[i].color = Color.yellow;
            }
            else
            {
                CostData costData = node.costs[i];
                float currentCost = costData.GetCurrentCost(node.currentUpgradeAmount);
                spawnedCostTexts[i].text = currentCost.ToString();
                spawnedCostTexts[i].color = (PlayerResources.instance.GetResourceAmount(costData.resourceType) >= currentCost) ? Color.white : Color.red;
            }
        }
    }
}