using UnityEngine;

public class Drill : Weapon
{
    [SerializeField] private float drillRadius = 1f;
    [SerializeField] private float offset = 4f;
    [SerializeField] private LayerMask resourceLayer;

    public bool BuffedDrillActive = false;

    private void Start()
    {
        if (BuffedDrillActive)
        {
            OPDrill();
        }
    }
    protected override void Update()
    {
        base.Update();
        transform.localScale = new Vector3(drillRadius * offset, drillRadius * offset, 1);
    }
    public override void Fire()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(firepoint.position, drillRadius, resourceLayer);

        foreach (Collider2D col in hitColliders)
        {
            ResourceTile resource = col.GetComponent<ResourceTile>();

            if (resource != null)
            {
                resource.TakeDamage(damage);
            }
        }
    }

    [ContextMenu("Overpowered Drill")]
    public void OPDrill()
    {
        drillRadius = 3f;
        damage = 30f;
        attackSpeed = 15f;
    }

    private void OnDrawGizmosSelected()
    {
        if (firepoint == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(firepoint.position, drillRadius);
    }
}

