using UnityEngine;

[CreateAssetMenu(fileName = "InventoryUI", menuName = "Scriptable Objects/InventoryUI")]
public class InventoryUISO : ScriptableObject
{
    public Resource resourceType;
    public string resourceName;
    public Sprite resourceIcon;
    public int sortOrder;
}
