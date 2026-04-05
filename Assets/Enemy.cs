using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    protected float maxHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected float stopDistance;
    [SerializeField] protected float aggroRange = 3f;
    [SerializeField] protected float patrolSpeed = 0.1f;

    [SerializeField] protected GameObject droppedResource;

    protected Transform player;
    protected Vector3 startPosition;

    protected enum EnemyStates { Patrol, Moving, Attacking }
    protected EnemyStates currentState;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        currentState = EnemyStates.Patrol;
        maxHealth = health;
    }

    protected virtual void HandlePatrol()
    {
        float patrolRange = 5f;
        float pingPong = Mathf.PingPong(Time.time * patrolSpeed, patrolRange) - (patrolRange / 2f);
        transform.position = startPosition + new Vector3(pingPong, 0, 0);
    }

    protected virtual void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentState != EnemyStates.Attacking)
        {
            if (distanceToPlayer <= aggroRange)
                currentState = EnemyStates.Moving;
            else
                currentState = EnemyStates.Patrol;
        }

        switch (currentState)
        {
            case EnemyStates.Patrol:
                HandlePatrol();
                break;
            case EnemyStates.Moving:
                HandleMovement();
                break;
            case EnemyStates.Attacking:
                HandleAttack();
                break;
        }
    }

    protected abstract void HandleMovement();
    protected abstract void HandleAttack();

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    protected void Die()
    {
        DropLoot();
        Destroy(gameObject);
    }

    protected void DropLoot()
    {
        Instantiate(droppedResource, transform.position, Quaternion.identity);
    }
}