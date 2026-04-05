using UnityEngine;

public class Drill : Weapon
{
    [SerializeField] private float drillRadius = 1f;
    [SerializeField] private float offset = 5f;
    [SerializeField] private LayerMask resourceLayer;

    public bool BuffedDrillActive = false;
    public bool weaponActive = true;

    private void Start()
    {
        InitializeStats();
    }
    protected override void Update()
    {
        base.Update();
        transform.localScale = new Vector3(drillRadius * offset, drillRadius * offset, 1);
        this.enabled = weaponActive;
        GetComponent<SpriteRenderer>().enabled = weaponActive;
    }
    public override void Fire()
    {
        int combinedLayerMask = resourceLayer | (1 << 9);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(firepoint.position, drillRadius, combinedLayerMask);

        foreach (Collider2D col in hitColliders)
        {

            ResourceTile resource = col.GetComponent<ResourceTile>();
            if (resource != null)
            {
                resource.TakeDamage(damage);
            }

            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firepoint == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(firepoint.position, drillRadius);
    }

    public void InitializeStats()
    {
        drillRadius = PlayerStats.instance.drillRadius;
        damage = PlayerStats.instance.drillDamage;
        attackSpeed = PlayerStats.instance.drillAttackSpeed;
    }
}

