using UnityEngine;
using System.Collections.Generic;

public class SupportEnemy : Enemy
{
    [Header("Support Settings")]
    [SerializeField] private float healAmount = 10f;
    [SerializeField] private float healRate = 2f;
    private float nextHealTime;

    private Transform targetAlly;

    protected override void Start()
    {
        base.Start(); // Runs the player-finding logic from the base class
        FindNewAlly();
    }

    protected override void HandleMovement()
    {
        // If the ally is dead or missing, find a new one
        if (targetAlly == null)
        {
            FindNewAlly();
            return;
        }

        float distanceToAlly = Vector3.Distance(transform.position, targetAlly.position);

        // Move toward the ally, but stop at 'stopDistance' to stay behind them
        if (distanceToAlly > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetAlly.position, speed * Time.deltaTime);
        }
        else
        {
            // Once in position, switch to attacking (healing)
            currentState = EnemyStates.Attacking;
        }
    }

    protected override void HandleAttack()
    {
        if (targetAlly == null)
        {
            currentState = EnemyStates.Moving;
            return;
        }


        if (Time.time >= nextHealTime)
        {
            targetAlly.GetComponent<Enemy>().Heal(healAmount);

            nextHealTime = Time.time + healRate;
        }

        // If the ally moves too far away, go back to moving
        if (Vector3.Distance(transform.position, targetAlly.position) > stopDistance + 1f)
        {
            currentState = EnemyStates.Moving;
        }
    }

    private void FindNewAlly()
    {
        // Find all objects with the Enemy component
        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        float closestDistance = Mathf.Infinity;

        foreach (Enemy e in allEnemies)
        {

            if (e.transform == this.transform) continue;

            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                targetAlly = e.transform;
            }
        }
    }
}