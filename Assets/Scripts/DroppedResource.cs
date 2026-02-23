using UnityEngine;

public class DroppedResource : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float force = 10f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector3(Random.Range(-1f, 1f) * force, Random.Range(-1f, 1f) * force, 0));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
