using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject baseTile;
    [SerializeField] private ResourceTileSO[] resources;

    public int planetRadius = 10;

    private void Start()
    {
        GeneratePlanet(planetRadius);
    }
    public void GeneratePlanet(int radius)
    {
        Vector3 center = transform.position;

        // Outer loop for the Vertical (Y) axis
        for (int y = 0; y < radius; y++)
        {
            // Inner loop for the Horizontal (X) axis
            for (int x = 0; x < radius; x++)
            {
                // x moves right, -y moves downward
                Vector2 grid = new Vector2(center.x + x / 2f, center.y - y / 2f);

                GenerateTile(resources[3], grid);
            }
        }
    }

    private void GenerateTile(ResourceTileSO resourceType, Vector2 location)
    {
        GameObject generatedTile = Instantiate(baseTile, new Vector3(location.x, location.y, 0), Quaternion.identity);
        generatedTile.GetComponent<ResourceTile>().tileStats = resourceType;
    }
}
