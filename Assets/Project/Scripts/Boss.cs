using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] protected float bossHealth = 1000f;
    protected float bossMaxHealth;
    [SerializeField] protected GameObject bossLootDrop;
    [SerializeField] protected BossStates currentState;

    protected enum BossStates
    {
        MOVING,
        ATTACKING,
        IDLE,
    }

    private void Awake()
    {
        bossMaxHealth = bossHealth;
    }

    private void Start()
    {
        StartCoroutine(BossBehaviorLoop());
    }

    // A main loop that manages transitions between states
    protected IEnumerator BossBehaviorLoop()
    {
        while (bossHealth > 0)
        {
            switch (currentState)
            {
                case BossStates.IDLE:
                    yield return new WaitForSeconds(2f);
                    currentState = BossStates.MOVING;
                    break;

                case BossStates.MOVING:
                    // Add movement logic here
                    yield return new WaitForSeconds(3f);
                    currentState = BossStates.ATTACKING;
                    break;

                case BossStates.ATTACKING:
                    yield return StartCoroutine(AttackPattern());
                    currentState = BossStates.IDLE;
                    break;
            }
        }
    }

    protected virtual IEnumerator AttackPattern()
    {
        Debug.Log("Boss is winding up...");
        yield return new WaitForSeconds(1f);

        PerformAttack();

        yield return new WaitForSeconds(1.5f);
        Debug.Log("Boss is recovering.");
    }

    private void PerformAttack()
    {
        Debug.Log("Attack Released!");
    }

    public void TakeDamage(float amount)
    {
        bossHealth -= amount;
        if (bossHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        if (bossLootDrop != null)
        {
            Instantiate(bossLootDrop, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public float GetHealthNormalized()
    {
        return bossHealth / bossMaxHealth;
    }
}