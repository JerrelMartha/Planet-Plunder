using TMPro;
using UnityEngine;

public class InventoryUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] textComponents;
    [SerializeField] private InventoryUISO[] inventoryUISOs;

    private void Update()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        int index = 0;
        foreach (var item in textComponents)
        {
            ResourceCardUI cardUI = item.GetComponent<ResourceCardUI>();

            cardUI.so = inventoryUISOs[index];
            cardUI.UpdateInfo();

            index++;
        }
    }
}
