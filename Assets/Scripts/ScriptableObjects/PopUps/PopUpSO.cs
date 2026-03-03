using UnityEngine;

[CreateAssetMenu(fileName = "PopUp", menuName = "Scriptable Objects/PopUp")]
public class PopUpSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public Tiers itemTier;
}

    public enum Tiers 
    { 
        Tier1,
        Tier2,
        Tier3,
    }

