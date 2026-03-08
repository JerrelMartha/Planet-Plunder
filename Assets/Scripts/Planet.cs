using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject baseTile;
    [SerializeField] private ResourceTileSO[] resources;

    [Header("Noise Settings")]
    [SerializeField] private float noiseScale = 0.2f;   // Lower creates smoother edges, higher creates harsher edges
    [SerializeField] private float noiseIntensity = 2.0f; // Makes edges more intense
    [SerializeField] private float seed;

    [Header("Layer Spawn Chance")] // Chance for ores to spawn instead of stone
    [SerializeField] private float Layer3Chance = 0.3f; // Iron
    [SerializeField] private float Layer4Chance = 0.2f; // Gold
    [SerializeField] private float Layer5Chance = 0.1f; // Diamond

    public float planetRadius = 10;

    private void Start()
    {
        seed = Random.Range(0, 10000);
        GeneratePlanet(planetRadius);
    }

    // Destroys previous planet and generates a new one
    [ContextMenu("Random Planet")]
    public void RandomPlanet()
    {
        
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Resource");
        foreach (var item in tiles) Destroy(item);

        seed = Random.Range(0, 10000);
        GeneratePlanet(Random.Range(5f, 20f));
    }

    public void GeneratePlanet(float radius)
    {

        Vector2[] points = GetPointsInCircleWithNoise(radius);

        foreach (var item in points)
        {
            float pNoise = Mathf.PerlinNoise((item.x + seed) * noiseScale, (item.y + seed) * noiseScale);
            float noiseDistance = item.magnitude + (pNoise * 2 - 1) * noiseIntensity;

            if (noiseDistance >= radius * 0.8f)
            {
                // Layer 1
                GenerateTile(resources[0], item);
            }
            else if (noiseDistance >= radius * 0.6f)
            {
                // Layer 2
                GenerateTile(resources[1], item);
            }
            else if (noiseDistance >= radius * 0.4f)
            {
                var res = (Random.value < Layer3Chance) ? resources[2] : resources[1];
                GenerateTile(res, item);
            }
            else if (noiseDistance >= radius * 0.2f)
            {
                var res = (Random.value < Layer4Chance) ? resources[3] : resources[1];
                GenerateTile(res, item);
            }
            else
            {
                var res = (Random.value < Layer5Chance) ? resources[4] : resources[1];
                GenerateTile(res, item);
            }
        }
    }

    // Gets all the points off the grid that are inside of the planets radius with noise
    private Vector2[] GetPointsInCircleWithNoise(float radius)
    {
        List<Vector2> points = new List<Vector2>();

        float iterationRange = radius + noiseIntensity;
        float step = 0.5f;

        for (float x = -iterationRange; x <= iterationRange; x += step)
        {
            for (float y = -iterationRange; y <= iterationRange; y += step)
            {

                float pNoise = Mathf.PerlinNoise((x + seed) * noiseScale, (y + seed) * noiseScale);


                float offset = (pNoise * 2 - 1) * noiseIntensity;


                float noisyRadius = radius + offset;


                if (Vector2.Distance(Vector2.zero, new Vector2(x, y)) <= noisyRadius)
                {
                    points.Add(new Vector2(x, y));
                }
            }
        }
        return points.ToArray();
    }

    // Generates a tile with a resource type and puts it at a location
    private void GenerateTile(ResourceTileSO resourceType, Vector2 location)
    {
        GameObject generatedTile = Instantiate(baseTile, new Vector3(location.x, location.y, 0), Quaternion.identity);
        generatedTile.GetComponent<ResourceTile>().tileStats = resourceType;
        generatedTile.transform.parent = transform;
    }
}