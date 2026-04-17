using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resourceNameText;
    [SerializeField] private TextMeshProUGUI resourceValueText;
    [SerializeField] private Image resourceIconImage;
    public InventoryUISO so;

    private void OnEnable()
    {
        PlayerResources.TemporaryResourceAdded += UpdateInfo;
    }

    private void OnDisable()
    {
        PlayerResources.TemporaryResourceAdded -= UpdateInfo;
    }

    public void UpdateInfo()
    {
        if (so == null || resourceNameText == null || resourceValueText == null || resourceIconImage == null)
        {
            return;
        }

        float amount = PlayerResources.instance.GetResourceAmount(so.resourceType);

        resourceNameText.text = so.resourceName;
        resourceValueText.text = HelperFunctions.FormatNumber(amount);
        resourceIconImage.sprite = so.resourceIcon;
        gameObject.name = so.resourceName;
    }
}