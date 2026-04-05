using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    protected float maxHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected float stopDistance;

    [SerializeField] protected GameObject droppedResource;

    protected Transform player;

    protected enum EnemyStates { Patrol, Moving, Attacking }
    protected EnemyStates currentState;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = EnemyStates.Moving;
        maxHealth = health;
    }

    public void Heal(float amount)
    {
        health += amount;

        if (health >  maxHealth)
        {
            health = maxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }


    protected void Die()
    {
        DropLoot();
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyStates.Moving:
                HandleMovement();
                break;
            case EnemyStates.Attacking:
                HandleAttack();
                break;
        }
    }

    // This is what we will override in child classes
    protected abstract void HandleMovement();
    protected abstract void HandleAttack();

    protected void DropLoot()
    {
        Instantiate(droppedResource, transform.position, Quaternion.identity);
    }
}