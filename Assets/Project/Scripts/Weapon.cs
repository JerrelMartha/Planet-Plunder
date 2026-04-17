using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float cost;
    [Tooltip("2 attackSpeed = 2 projectiles per second")]
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform firepoint;

    [SerializeField] protected bool weaponUnlocked = true;
    [SerializeField] public string weaponID;

    private float nextFireTime;
    private bool isHoldingFire;

    protected virtual void OnEnable()
    {
        if (WeaponManager.instance != null && WeaponManager.instance.IsIDUnlocked(weaponID))
        {
            weaponUnlocked = true;
        }

        if (!weaponUnlocked)
        {
            enabled = false;
        }
    }

    protected virtual void OnDisable()
    {
        isHoldingFire = false;
    }

    public void TryFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) isHoldingFire = true;
        if (ctx.canceled) isHoldingFire = false;
    }

    protected virtual void Update()
    {
        if (isHoldingFire && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + (1f / attackSpeed);
        }
    }

    public virtual void Fire()
    {
        if (projectile != null && firepoint != null)
        {
            Fuel.instance.RemoveFuel(cost);
            Debug.Log(cost);
            Instantiate(projectile, firepoint.position, firepoint.rotation);
        }
    }

    public bool IsWeaponUnlocked()
    {
        return weaponUnlocked;
    }

    public void Unlock()
    {
        weaponUnlocked = true;
        enabled = true;
    }
}