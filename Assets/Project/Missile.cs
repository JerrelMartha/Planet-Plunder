using UnityEngine;

public class Missile : Weapon
{
    protected float missileArea;

    protected virtual void Start()
    {
        InitializeStats();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Fire()
    {
        if (projectile == null || firepoint == null) return;

        GameObject obj = Instantiate(projectile, firepoint.position, firepoint.rotation);
        MissileProjectile missile = obj.GetComponent<MissileProjectile>();

        if (missile != null)
        {
            missile.missileDamage = damage;
            missile.missileSpeed = bulletSpeed;
            missile.missileArea = missileArea;
        }

        if (Fuel.instance != null)
        {
            Fuel.instance.RemoveFuel(cost);
        }
    }

    public void InitializeStats()
    {
        if (PlayerStats.instance != null)
        {
            damage = PlayerStats.instance.missileDamage;
            bulletSpeed = PlayerStats.instance.missileBulletSpeed;
            attackSpeed = PlayerStats.instance.missileAttackSpeed;
            missileArea = PlayerStats.instance.missileArea;
            cost = PlayerStats.instance.missileCost;
        }
    }
}