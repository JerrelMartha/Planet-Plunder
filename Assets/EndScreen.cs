using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EndScreen : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject resourceRowPrefab;
    [SerializeField] private Transform container;

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

        if (tempInv == null || tempInv.Count == 0)
        {
            return;
        }

        foreach (var entry in tempInv)
        {
            if (entry.Value <= 0) continue;

            GameObject row = Instantiate(resourceRowPrefab, container);
            TMP_Text text = row.GetComponentInChildren<TMP_Text>();

            if (text != null)
            {
                text.text = $"{entry.Key}: {entry.Value}";
            }
        }
    }
}