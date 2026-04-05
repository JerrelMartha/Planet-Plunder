using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private EnemyProjectileSO projectileData;

    private void Start()
    {
        transform.localScale = projectileData.size;
        GetComponent<SpriteRenderer>().sprite = projectileData.projectileImage;
        GetComponent<Rigidbody2D>().linearVelocity = PlayerUtils.Instance.GetDirectionToPlayer(transform.position) * projectileData.speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<Fuel>().RemoveFuel(GetDamage());
            Destroy(gameObject);
        }
    }

    public float GetDamage() => projectileData.damage;
}