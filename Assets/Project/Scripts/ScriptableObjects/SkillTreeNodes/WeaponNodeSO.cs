using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponNode", menuName = "Scriptable Objects/WeaponNodeSO")]
public class WeaponNodeSO : NodeSO
{
    [Header("Weapon Settings")]
    public string weaponID;
}