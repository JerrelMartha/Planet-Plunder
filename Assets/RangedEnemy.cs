using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackSpeed = 1.5f;
    [SerializeField] private Transform firePoint;

    private float lastAttackTime;

    protected override void HandleMovement()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            currentState = EnemyStates.Attacking;
        }
    }

    protected override void HandleAttack()
    {
        if (Time.time >= lastAttackTime + attackSpeed)
        {
            Shoot();
            lastAttackTime = Time.time;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > stopDistance)
        {
            currentState = EnemyStates.Moving;
        }
    }

    private void Shoot()
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Vector3 direction = (player.position - spawnPos).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject obj = Instantiate(projectile, spawnPos, rotation);
        Destroy(obj, 2f);
    }
}