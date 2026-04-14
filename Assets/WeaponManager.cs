using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [SerializeField] private List<string> unlockedWeaponIDs = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (var item in unlockedWeaponIDs)
        {
            UnlockWeapon(item);
        }
    }

    public void UnlockWeapon(string id)
    {
        if (!unlockedWeaponIDs.Contains(id))
        {
            unlockedWeaponIDs.Add(id);
        }
    }

    public bool IsIDUnlocked(string id)
    {
        return unlockedWeaponIDs.Contains(id);
    }

    public List<string> GetUnlockedWeaponIDs()
    {
        return new List<string>(unlockedWeaponIDs);
    }

    public void LoadData(List<string> savedIDs)
    {
        if (savedIDs != null)
        {
            unlockedWeaponIDs = savedIDs;
        }
    }
}