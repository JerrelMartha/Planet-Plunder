using UnityEngine;

public class ClusterBomb : Missile
{
    [SerializeField] private int clusters = 3;
    [SerializeField] private float spreadAngle = 15f;

    [ContextMenu("Fire")]
    public override void Fire()
    {
        base.Fire();

        for (int i = 0; i < clusters; i++)
        {
            Quaternion randomRotation = firepoint.rotation * Quaternion.Euler(0, 0, Random.Range(-spreadAngle, spreadAngle));
            GameObject obj = Instantiate(projectile, firepoint.position, randomRotation);

            MissileProjectile missile = obj.GetComponent<MissileProjectile>();
            if (missile != null)
            {
                missile.missileDamage = damage;
                missile.missileSpeed = bulletSpeed;
                missile.missileArea = missileArea;
            }
        }
    }
}