using System.Collections;
using UnityEngine;

public class MissileProjectile : MonoBehaviour
{
    public float missileArea;
    public float missileSpeed;
    public float missileDamage;
    private float missileLifetime = 3f;
    [SerializeField] private GameObject particles;
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
        if (collision.gameObject.layer == 9) // Enemy Layer
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(missileDamage); 
        }

        if (collision.gameObject.layer == 11) // BossLayer
        {
            collision.gameObject.GetComponent<Boss>().TakeDamage(missileDamage);
        }
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

        SpawnParticles();
        Destroy(gameObject);
    }

    private IEnumerator Expire(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Die();
    }

    private void SpawnParticles()
    {
        GameObject prt = Instantiate(particles, transform.position, Quaternion.identity);

        ParticleSystem ps = prt.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            var main = ps.main;

            float minSpeed = missileArea * 1.5f;
            float maxSpeed = missileArea * 2f;

            main.startSpeed = new ParticleSystem.MinMaxCurve(minSpeed, maxSpeed);
        }

        Destroy(prt, 2f);
    }
}