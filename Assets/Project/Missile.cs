using UnityEngine;

public class Missile : Weapon
{
    private float missileArea;

    public bool weaponActive = true;

    private void Start()
    {
        InitializeStats();
        
    }

    protected override void Update()
    {
        base.Update();
        this.enabled = weaponActive;
    }

    public override void Fire()
    {
        GameObject obj = Instantiate(projectile, firepoint.position, firepoint.rotation);
        MissileProjectile missile = obj.GetComponent<MissileProjectile>();

        missile.missileDamage = damage;
        missile.missileSpeed = bulletSpeed;
        missile.missileArea = missileArea;
    }

    public void InitializeStats()
    {
        if (PlayerStats.instance != null)
        {
            damage = PlayerStats.instance.missileDamage;
            bulletSpeed = PlayerStats.instance.missileBulletSpeed;
            attackSpeed = PlayerStats.instance.missileAttackSpeed;
            missileArea = PlayerStats.instance.missileArea;
        }

    }
}