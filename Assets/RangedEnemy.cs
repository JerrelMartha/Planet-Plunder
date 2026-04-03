using UnityEngine;

public class RangedEnemy : Enemy
{
    protected override void HandleMovement()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            // Stop moving and start shooting
            currentState = EnemyStates.Attacking;
        }
    }

    protected override void HandleAttack()
    {
        // Shooting logic here
        Debug.Log("Firing at player from a distance!");

        // If the player gets too close or too far, return to Moving
        if (Vector3.Distance(transform.position, player.position) != stopDistance)
            currentState = EnemyStates.Moving;
    }
}