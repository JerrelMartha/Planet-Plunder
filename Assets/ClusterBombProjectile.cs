using UnityEngine;

public class ClusterBombProjectile : MissileProjectile
{
    [Header("Cluster Settings")]
    [SerializeField] private GameObject clusterPrefab;
    [SerializeField] public int clusterAmount = 5;
    [SerializeField] private float speedMultiplier = 1.2f;
    [SerializeField] private bool canSpawnClusters = true;

    protected override void Die()
    {
        if (canSpawnClusters)
        {
            SpawnClusters();
        }
        base.Die();
    }

    private void SpawnClusters()
    {
        for (int i = 0; i < clusterAmount; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            GameObject cluster = Instantiate(clusterPrefab, transform.position, Quaternion.identity);

            if (cluster.TryGetComponent(out ClusterBombProjectile clusterScript))
            {
                clusterScript.canSpawnClusters = false;
                clusterScript.missileDamage = missileDamage / 2f;
                clusterScript.missileSpeed = missileSpeed * speedMultiplier;
                clusterScript.missileArea = missileArea;
                clusterScript.SetDirection(randomDirection);
            }
            else if (cluster.TryGetComponent(out MissileProjectile baseScript))
            {
                baseScript.missileDamage = missileDamage / 2f;
                baseScript.missileSpeed = missileSpeed * speedMultiplier;
                baseScript.missileArea = missileArea;
                baseScript.SetDirection(randomDirection);
            }
        }
    }
}