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
        spriteRenderer.sprite = tileStats.tileSprite;
        spriteRenderer.color = tileStats.color;
    }

    
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }

        Color darkenedColor = tileStats.color * (55f / 255f);

        darkenedColor.a = tileStats.color.a;

        spriteRenderer.color = Color.Lerp(darkenedColor, tileStats.color, GetHealthNormalized());

        SpawnParticles();
    }

    [ContextMenu("die")]
    private void Die()
    {
        GameObject droppedResource = Instantiate(tileStats.droppedResource, transform.position, Quaternion.identity);
        droppedResource.transform.parent = transform.parent; // Planet
        Destroy(gameObject);
    }

    public void SpawnParticles()
    {
        GameObject prt = Instantiate(tileStats.particles, transform.position, Quaternion.identity);
        prt.transform.parent = transform;
        Destroy(prt, 2f);
    }

    private float GetHealthNormalized()
    {
        return health / tileStats.health;
    }
}
