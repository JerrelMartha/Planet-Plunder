using UnityEngine;

[CreateAssetMenu(fileName = "ResourceTileSO", menuName = "Scriptable Objects/ResourceTile")]
public class ResourceTileSO : ScriptableObject
{
    public string resourceName;
    public Sprite tileSprite;
    public GameObject droppedResource;
    public GameObject particles;
    public Color color;
    public float health;
}
