using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject baseTile;
    [SerializeField] private ResourceTileSO[] resources;
    private Vector3 center;

    [SerializeField] private float noiseScale = 0.5f;
    [SerializeField] private float noiseIntensity = 1.5f;
    [SerializeField] private float seed; // Randomize this for different planets

    public float planetRadius = 10;

    private void Start()
    {
        center = transform.position;
        GeneratePlanet(planetRadius);
    }

    [ContextMenu("Random Planet")]
    public void RandomPlanet()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Resource");
        foreach (var item in tiles)
        {
            Destroy(item);
        }
        GeneratePlanet(Random.Range(5f, 40f));
    }

    // Places Tiles within all position inside radius. Resource gets chosen based on depth and perlin noise
    public void GeneratePlanet(float radius)
    {
        Vector2[] points = GetPointsInCircle(radius); // Gets all points of the grid inside of the radius
        seed = Random.Range(0, 1000); // Generates a random seed

        foreach (var item in points)
        {
            float pNoise = Mathf.PerlinNoise(
                (item.x + seed) * noiseScale,
                (item.y + seed) * noiseScale
            );

            float noiseDistance = item.magnitude + (pNoise * 2 - 1) * noiseIntensity;

            if (noiseDistance >= radius * 0.8f)
            {
                GenerateTile(resources[0], item);
            }
            else if (noiseDistance >= radius * 0.6f)
            {
                GenerateTile(resources[1], item);
            }
            else if (noiseDistance >= radius * 0.4f)
            {
                GenerateTile(resources[2], item);
            }
            else if (noiseDistance >= radius * 0.2f)
            {
                GenerateTile(resources[3], item);
            }
            else
            {
                GenerateTile(resources[4], item);
            }
        }
    }

    private Vector2[] GetPointsInCircle(float radius)
    {
        List<Vector2> points = new List<Vector2>();

        // Snap the radius to the nearest 0.5 increment
        float snappedRadius = Mathf.Round(radius * 2f) / 2f;
        float rSquared = snappedRadius * snappedRadius;

        // We use a small epsilon (0.001f) to avoid floating point precision errors
        // ensuring we don't skip the outer boundary of the circle.
        for (float x = -snappedRadius; x <= snappedRadius + 0.001f; x += 0.5f)
        {
            for (float y = -snappedRadius; y <= snappedRadius + 0.001f; y += 0.5f)
            {
                // Standard circle equation: x² + y² <= r²
                if (x * x + y * y <= rSquared + 0.001f)
                {
                    points.Add(new Vector2(x, y));
                }
            }
        }

        return points.ToArray();
    }

    private void GenerateTile(ResourceTileSO resourceType, Vector2 location)
    {
        GameObject generatedTile = Instantiate(baseTile, new Vector3(location.x, location.y, 0), Quaternion.identity);
        generatedTile.GetComponent<ResourceTile>().tileStats = resourceType;
    }
}
