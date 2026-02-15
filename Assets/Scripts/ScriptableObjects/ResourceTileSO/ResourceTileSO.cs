using UnityEngine;

[CreateAssetMenu(fileName = "ResourceTileSO", menuName = "Scriptable Objects/ResourceTile")]
public class ResourceTileSO : ScriptableObject
{
    public string resourceName;
    public GameObject droppedResource;
    public Color color;
    public float health;
}
