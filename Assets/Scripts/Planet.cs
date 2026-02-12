using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject baseTile;
    [SerializeField] private ResourceTileSO[] resources;
    public void GenerateTile(ResourceTileSO resourceType, Vector2 location)
    {
        GameObject generatedTile = Instantiate(baseTile, new Vector3(location.x, location.y, 0), Quaternion.identity);
        generatedTile.GetComponent<ResourceTile>().tileStats = resourceType;
    }
}
