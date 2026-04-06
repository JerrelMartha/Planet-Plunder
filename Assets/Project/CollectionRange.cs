using UnityEngine;

public class CollectionRange : MonoBehaviour
{
    [SerializeField] private float range = 1.5f;
    [SerializeField] private float pullStrength = 10f;
    [SerializeField] private LayerMask resourceLayer;
    [SerializeField] private bool updateRangePerFrame = false;

    private void Start()
    {
        InitializeStats();
        UpdateRange();
    }

    private void Update()
    {
        if (updateRangePerFrame)
        {
            UpdateRange();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & resourceLayer) != 0)
        {
            if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized;
                rb.AddForce(direction * pullStrength, ForceMode2D.Force);
            }
        }
    }

    private void UpdateRange()
    {
        transform.localScale = new Vector3(range, range, 1);
    }

    public void InitializeStats()
    {
        range = PlayerStats.instance.collectionRange;
    }
}
