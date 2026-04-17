using UnityEngine;

public class ClusterBombProjectile : MissileProjectile
{
    [Header("Cluster Settings")]
    [SerializeField] private GameObject clusterPrefab;
    [SerializeField] private int clusterAmount = 5;
    [SerializeField] private float spreadAngle = 45f;
    [SerializeField] private float speedMultiplier = 1.2f;

    protected override void Die()
    {
        SpawnClusters();
        base.Die();
    }

    private void SpawnClusters()
    {
        Vector2 travelDirection = transform.up;
        float startAngle = -spreadAngle / 2f;
        float angleStep = clusterAmount > 1 ? spreadAngle / (clusterAmount - 1) : 0;

        for (int i = 0; i < clusterAmount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Vector2 finalDirection = rotation * travelDirection;

            GameObject cluster = Instantiate(clusterPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, finalDirection));

            MissileProjectile clusterScript = cluster.GetComponent<MissileProjectile>();
            if (clusterScript != null)
            {
                clusterScript.missileDamage = missileDamage / 2f;
                clusterScript.missileSpeed = missileSpeed * speedMultiplier;
                clusterScript.missileArea = missileArea;
            }

            Rigidbody2D rb = cluster.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = finalDirection * (missileSpeed * speedMultiplier);
            }
        }
    }
}