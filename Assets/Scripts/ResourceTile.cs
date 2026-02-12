using UnityEngine;

public class ResourceTile : MonoBehaviour
{
    [SerializeField] public ResourceTileSO tileStats;
    [SerializeField] private float health;

    private void Awake()
    {
        health = tileStats.health;
    }
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }

        // Hit effect & Sounds
    }

    [ContextMenu("die")]
    public void Die()
    {
        Instantiate(tileStats.droppedResource, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
