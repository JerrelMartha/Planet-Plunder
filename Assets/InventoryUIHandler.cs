using UnityEngine;

public class InventoryUIHandler : MonoBehaviour
{
    public static InventoryUIHandler instance;
    [SerializeField] private GameObject resourceCard;

    [SerializeField] private InventoryUISO[] resources;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        } else
        {
            Destroy(gameObject);
        }
    }
    public void SpawnCard(Resource resourceType, int resourceSO)
    {
        GameObject card = Instantiate(resourceCard);
        card.transform.SetParent(transform);
        card.GetComponent<ResourceCardUI>().so = resources[resourceSO];
    }
}
