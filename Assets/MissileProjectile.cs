using System.Collections;
using UnityEngine;

public class MissileProjectile : MonoBehaviour
{
    public float missileArea;
    public float missileSpeed;
    public float missileDamage;
    private float missileLifetime = 3f;
    [SerializeField] private LayerMask tileLayer;

    private Vector2 direction;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(Expire(missileLifetime));
        
        Vector2 targetPos = HelperFunctions.GetMouseWorldPosition();

        
        direction = (targetPos - (Vector2)transform.position).normalized;

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate()
    {
        
        rb.linearVelocity = direction * missileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    private void Die()
    {
        Collider2D[] tiles = Physics2D.OverlapCircleAll(transform.position, missileArea, tileLayer);

        foreach (var item in tiles)
        {
            if (item.TryGetComponent<ResourceTile>(out ResourceTile tile))
            {
                tile.TakeDamage(missileDamage);
            }
        }
        Destroy(gameObject);
    }

    private IEnumerator Expire(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Die();
    }
}