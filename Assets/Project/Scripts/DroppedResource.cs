using UnityEngine;

public class DroppedResource : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float force = 10f;
    [SerializeField] private int combinedAmount = 1;
    [SerializeField] private Resource resourceType;

    [Header("Merging Settings")]
    [SerializeField] private float scaleMultiplier = 0.1f;
    [SerializeField] private float baseScale = 0.2f;
    [SerializeField] private float mergeDelay = 0.5f;
    [SerializeField] private int maxAmount = 10;

    private float mergeTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * force, ForceMode2D.Impulse);

        UpdateVisualScale();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (combinedAmount >= maxAmount) return;

        if (collision.TryGetComponent<DroppedResource>(out DroppedResource otherResource))
        {
            if (otherResource.resourceType == this.resourceType && GetInstanceID() > otherResource.GetInstanceID())
            {
                mergeTimer += Time.deltaTime;

                if (mergeTimer >= mergeDelay)
                {
                    Merge(otherResource);
                    mergeTimer = 0f;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<DroppedResource>(out DroppedResource otherResource))
        {
            if (otherResource.resourceType == this.resourceType)
            {
                mergeTimer = 0f;
            }
        }
    }

    private void Merge(DroppedResource other)
    {
        int spaceLeft = maxAmount - combinedAmount;
        int amountToAdd = Mathf.Min(spaceLeft, other.combinedAmount);

        combinedAmount += amountToAdd;
        other.combinedAmount -= amountToAdd;

        UpdateVisualScale();

        if (other.combinedAmount <= 0)
        {
            Destroy(other.gameObject);
        }
        else
        {
            other.UpdateVisualScale();
        }
    }

    private void UpdateVisualScale()
    {
        float newScale = baseScale + (combinedAmount * scaleMultiplier);
        transform.localScale = new Vector3(newScale, newScale, 1f);
    }

    public void Collect()
    {
        SoundManager.Instance.PlaySound("Collect", true);
        PlayerResources.instance.AddResource(resourceType, combinedAmount);
        PlayerResources.instance.AddTemporaryResource(resourceType, 1f);
        Destroy(gameObject);
    }
}