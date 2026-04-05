using UnityEngine;

public class CollectionRange : MonoBehaviour
{
    [SerializeField] private float range = 1.5f;
    [SerializeField] private float pullStrength = 10f;
    [SerializeField] private int resourceLayer = 3;
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
        if (collision.gameObject.layer == resourceLayer)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb.AddForce(direction * pullStrength);
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
