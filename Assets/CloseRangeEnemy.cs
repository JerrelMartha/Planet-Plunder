using UnityEngine;

public class CloseRangeEnemy : Enemy
{
    [SerializeField] private float contactDamage = 2f;
    [SerializeField] private float damageCooldown = 1f;
    private float nextDamageTime;

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
        
        if (Vector3.Distance(transform.position, player.position) > stopDistance)
        {
            currentState = EnemyStates.Moving;
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
        {


            collision.gameObject.GetComponent<Fuel>().RemoveFuel(contactDamage);
            nextDamageTime = Time.time + damageCooldown;
        }
    }
}