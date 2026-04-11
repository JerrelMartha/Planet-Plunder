using UnityEngine;

public class Drill : Weapon
{
    [SerializeField] private float drillEnemyDamage = 0.5f;
    [SerializeField] private float drillRadius = 1f;
    [SerializeField] private float offset = 5f;
    [SerializeField] private LayerMask resourceLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask bossLayer;

    public bool BuffedDrillActive = false;
    public bool weaponActive = true;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        InitializeStats();
    }

    protected override void Update()
    {
        base.Update();

        if (sr != null)
            sr.enabled = weaponActive;

        if (weaponActive)
        {
            transform.localScale = new Vector3(drillRadius * offset, drillRadius * offset, 1);
        }
    }

    public override void Fire()
    {
        if (!weaponActive) return;

        int combinedLayerMask = resourceLayer.value | enemyLayer.value | bossLayer.value;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(firepoint.position, drillRadius, combinedLayerMask);

        foreach (Collider2D col in hitColliders)
        {
            if (col.TryGetComponent(out ResourceTile resource))
            {
                resource.TakeDamage(damage);
            }

            if (col.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(drillEnemyDamage);
            }

            if (col.TryGetComponent(out Boss bossComponent))
            {
                bossComponent.TakeDamage(drillEnemyDamage);
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
        if (PlayerStats.instance == null) return;

        drillRadius = PlayerStats.instance.drillRadius;
        damage = PlayerStats.instance.drillDamage;
        attackSpeed = PlayerStats.instance.drillAttackSpeed;
        drillEnemyDamage = PlayerStats.instance.drillEnemyDamage;
    }
}