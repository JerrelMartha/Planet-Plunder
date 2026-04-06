using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndScreen : MonoBehaviour
{
    [System.Serializable]
    public struct ResourceIcon
    {
        public Resource resourceType;
        public Sprite iconSprite;
    }

    [Header("Setup")]
    [SerializeField] private GameObject resourceRowPrefab;
    [SerializeField] private Transform container;

    [Header("Icon Configuration")]
    [SerializeField] private List<ResourceIcon> resourceIcons;

    private void Start()
    {
        DisplayResources();
    }

    private void DisplayResources()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        var tempInv = PlayerResources.instance.GetTemporaryInventory();

        if (tempInv == null || tempInv.Count == 0) return;

        foreach (var entry in tempInv)
        {
            if (entry.Value <= 0) continue;

            GameObject row = Instantiate(resourceRowPrefab, container);

            TMP_Text text = row.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                text.text = HelperFunctions.FormatNumber(entry.Value);
            }

            Image iconImage = row.transform.Find("Image").GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = GetSpriteForResource(entry.Key);
            }
        }
    }

    private Sprite GetSpriteForResource(Resource type)
    {
        ResourceIcon match = resourceIcons.Find(x => x.resourceType == type);
        return match.iconSprite;
    }
}