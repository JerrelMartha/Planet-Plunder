using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Tier1Boss : Boss
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float contactDamage = 10f;
    private bool canHit = true;
    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        StartCoroutine(BossBehaviorLoop());
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case BossStates.MOVING:
                MoveTowardsPlayer();
                break;
            case BossStates.ATTACKING:
                FacePlayer();
                break;
            case BossStates.IDLE:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canHit) return;
        if (collision.gameObject.layer == 8)
        {
            if (collision.gameObject.TryGetComponent(out Fuel fuel))
            {
                fuel.RemoveFuel(contactDamage);
                StartCoroutine(AttackCooldown());
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        FacePlayer();
    }

    private void FacePlayer()
    {
        if (player == null) return;

        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    protected override IEnumerator AttackPattern()
    {
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.8f);

        if (player != null)
        {
            float dashSpeed = moveSpeed * 3f;
            Vector2 dashDir = (player.position - transform.position).normalized;
            rb.linearVelocity = dashDir * dashSpeed;
        }

        yield return new WaitForSeconds(1.5f);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.7f);
    }

    private IEnumerator AttackCooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(0.2f);
        canHit = true;
    }
}