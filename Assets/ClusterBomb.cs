using UnityEngine;

public class ClusterBomb : Missile
{
    [SerializeField] private int clusters = 3;

    [ContextMenu("Fire")]
    public override void Fire()
    {
        GameObject obj = Instantiate(projectile, firepoint.position, firepoint.rotation);

        if (obj.TryGetComponent(out ClusterBombProjectile clusterProjectile))
        {
            clusterProjectile.missileDamage = damage;
            clusterProjectile.missileSpeed = bulletSpeed;
            clusterProjectile.missileArea = missileArea;

            clusterProjectile.clusterAmount = this.clusters;
        }
    }
}