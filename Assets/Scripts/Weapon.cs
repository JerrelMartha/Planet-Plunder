using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float bulletSpeed;
    [Tooltip("2 attackSpeed = 2 projectiles per second")]
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform firepoint;

    protected bool canFire = true;

    private bool isHoldingFire;

    public void TryFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) isHoldingFire = true;
        if (ctx.canceled) isHoldingFire = false;
    }

    protected virtual void Update()
    {
        if (isHoldingFire && canFire)
        {
            Fire();
            StartCoroutine(WeaponCooldown());
        }
    }

    public virtual void Fire()
    {
        GameObject spawnedObject = Instantiate(projectile, firepoint);
        spawnedObject.transform.parent = firepoint;
    }
    protected IEnumerator WeaponCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(1 / attackSpeed);
        canFire = true;
    }
}
