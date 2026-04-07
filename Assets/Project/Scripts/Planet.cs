using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject baseTile;
    [SerializeField] private ResourceTileSO[] resources;

    [Header("Noise Settings")]
    [SerializeField] private float noiseScale = 0.2f;
    [SerializeField] private float noiseIntensity = 2.0f;
    [SerializeField] private float seed;

    [Header("Layer Spawn Chance")]
    [SerializeField] private float Layer3Chance = 0.3f;
    [SerializeField] private float Layer4Chance = 0.2f;
    [SerializeField] private float Layer5Chance = 0.1f;

    [Range(0f, 80f)]
    public float planetRadius = 10;

    [Header("Enemy Settings")]
    [SerializeField] private List<EnemySpawnData> Enemies;
    [SerializeField] private float enemySpawnHeight = 1.0f;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField, Range(0f, 1f)] private float globalSpawnDensity = 0.1f;

    private int currentEnemyCount;

    [System.Serializable]
    public struct EnemySpawnData
    {
        public float SpawnChance;
        public GameObject enemyPrefab;
    }

    private void Start()
    {
        seed = Random.Range(0f, 10000f);
        GeneratePlanet(planetRadius);
    }

    [ContextMenu("Random Planet")]
    public void RandomPlanet()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(transform.GetChild(i).gameObject);
            else
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        seed = Random.Range(0f, 10000f);
        GeneratePlanet(planetRadius);
    }

    public void GeneratePlanet(float radius)
    {
        currentEnemyCount = 0;
        float checkRange = radius + noiseIntensity + enemySpawnHeight;
        float step = 0.5f;

        for (float x = -checkRange; x <= checkRange; x += step)
        {
            for (float y = -checkRange; y <= checkRange; y += step)
            {
                Vector2 point = new Vector2(x, y);
                float pNoise = Mathf.PerlinNoise((x + seed) * noiseScale, (y + seed) * noiseScale);
                float offset = (pNoise * 2 - 1) * noiseIntensity;
                float noisyRadius = radius + offset;
                float distance = point.magnitude;

                if (distance <= noisyRadius)
                {
                    float layerDist = distance / noisyRadius;

                    if (layerDist >= 0.8f)
                    {
                        GenerateTile(resources[0], point);
                    }
                    else if (layerDist >= 0.6f)
                    {
                        GenerateTile(resources[1], point);
                    }
                    else if (layerDist >= 0.4f)
                    {
                        var res = (Random.value < Layer3Chance) ? resources[2] : resources[1];
                        GenerateTile(res, point);
                    }
                    else if (layerDist >= 0.2f)
                    {
                        var res = (Random.value < Layer4Chance) ? resources[3] : resources[1];
                        GenerateTile(res, point);
                    }
                    else
                    {
                        var res = (Random.value < Layer5Chance) ? resources[4] : resources[1];
                        GenerateTile(res, point);
                    }
                }
                else if (distance > noisyRadius && distance <= noisyRadius + step)
                {
                    if (currentEnemyCount < maxEnemies)
                    {
                        SpawnEnemy(point);
                    }
                }
            }
        }
    }

    private void GenerateTile(ResourceTileSO resourceType, Vector2 location)
    {
        Vector3 spawnPosition = new Vector3(location.x, location.y, 0) + transform.position;
        GameObject generatedTile = Instantiate(baseTile, spawnPosition, Quaternion.identity);
        generatedTile.GetComponent<ResourceTile>().tileStats = resourceType;
        generatedTile.transform.parent = transform;
    }

    private void SpawnEnemy(Vector2 pos)
    {
        
        if (currentEnemyCount >= maxEnemies) return;
        if (Random.value > globalSpawnDensity) return;

        float roll = Random.value;
        float cumulativeChance = 0f;

        for (int i = 0; i < Enemies.Count; i++)
        {
            cumulativeChance += Enemies[i].SpawnChance;

            if (roll <= cumulativeChance)
            {
                Vector3 spawnPos = (Vector3)(pos + pos.normalized * 0.5f) + transform.position;
                GameObject enemy = Instantiate(Enemies[i].enemyPrefab, spawnPos, Quaternion.identity);
                enemy.transform.parent = transform;

                currentEnemyCount++;
                return;
            }
        }
    }
}