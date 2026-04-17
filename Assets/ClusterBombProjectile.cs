using UnityEngine;

public class ClusterBombProjectile : MissileProjectile
{
    [Header("Cluster Settings")]
    [SerializeField] private GameObject clusterPrefab;
    [SerializeField] private int clusterAmount = 5;

    protected override void Die()
    {
        SpawnClusters();
        base.Die();
    }

    private void SpawnClusters()
    {
        float angleStep = 360f / clusterAmount;
        float angle = 0f;

        for (int i = 0; i < clusterAmount; i++)
        {
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 spawnDir = new Vector2(x, y);

            GameObject cluster = Instantiate(clusterPrefab, transform.position, Quaternion.identity);

            angle += angleStep;
        }
    }
}