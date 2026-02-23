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

        // 1. Calculate the 'Darkened' version (22% of original brightness)
        // This simulates putting a 200-alpha black layer over it.
        Color darkenedColor = tileStats.color * (55f / 255f);

        // 2. Ensure the Alpha stays at 1.0 (or whatever your tile's alpha is)
        darkenedColor.a = tileStats.color.a;

        // 3. Lerp between the full color and the darkened version
        spriteRenderer.color = Color.Lerp(darkenedColor, tileStats.color, GetHealthNormalized());
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
