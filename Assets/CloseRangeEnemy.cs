using UnityEngine;

public class CloseRangeEnemy : Enemy
{
    [SerializeField] private float contactDamage = 2f;
    [SerializeField] private float damageCooldown = 1f;
    private float nextDamageTime;

    protected override void HandleMovement()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Move towards player
        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            // Even if stopped, we stay in 'Moving' to keep the logic simple, 
            // or switch to Attacking if you have a specific animation.
            currentState = EnemyStates.Attacking;
        }
    }

    protected override void HandleAttack()
    {
        // If player kites the enemy and gets away, go back to chasing
        if (Vector3.Distance(transform.position, player.position) > stopDistance)
        {
            currentState = EnemyStates.Moving;
        }

        // Note: Actual damage is handled by the Physics engine below!
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if we hit the player and if enough time has passed
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
        {


            collision.gameObject.GetComponent<Fuel>().RemoveFuel(contactDamage);
            nextDamageTime = Time.time + damageCooldown;
        }
    }
}