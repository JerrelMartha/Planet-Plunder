using UnityEngine;

public class ResourceTile : MonoBehaviour
{
    [SerializeField] public ResourceTileSO tileStats;
    [SerializeField] private float health;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        health = tileStats.health;
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = tileStats.color;
    }

    
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }

        spriteRenderer.color = Color.Lerp(Color.black, tileStats.color, GetHealthNormalized());
        // Hit effect & Sounds
    }

    [ContextMenu("die")]
    public void Die()
    {
        Instantiate(tileStats.droppedResource, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private float GetHealthNormalized()
    {
        return health / tileStats.health;
    }
}
