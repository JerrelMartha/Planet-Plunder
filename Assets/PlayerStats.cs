using System;
using JetBrains.Annotations;
using UnityEngine;

public enum Stats 
{ 
    MoveSpeed,
    DashForce,
    BoostMult,
    DashCooldown,
    DashCost,
    MaxFuel,
    FuelSteal,
    DrillRadius,
    DrillAttackSpeed,
    DrillDamage,
    CollectionRange,
}

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float dashForce = 2f;
    public float boostMultiplier = 1.5f;
    public float dashCooldown = 2f;
    public float dashCost = 1f;


    [Header("Fuel")]
    public float maxFuel = 10f;
    public float fuelSteal = 0f;

    [Header("Drill")]
    public float drillRadius = 0.2f;
    public float drillAttackSpeed = 5f;
    public float drillDamage = 1f;

    [Header("Collection")]
    public float collectionRange = 1.5f;

    
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

        LoadData();
    }

    public void LoadData()
    {
        moveSpeed = PlayerPrefs.GetFloat("moveSpeed", 5f);
        dashForce = PlayerPrefs.GetFloat("dashForce", 2f);
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("moveSpeed", moveSpeed);
        PlayerPrefs.SetFloat("dashForce", dashForce);
    }

    public void IncreaseStat(Stats stats, float amount)
    {
        switch (stats)
        {
            case Stats.MoveSpeed:
                moveSpeed += amount;
                break;
            case Stats.DashForce:
                dashForce += amount;
                break;
            case Stats.BoostMult:
                boostMultiplier += amount;
                break;
            case Stats.DashCooldown:
                dashCooldown += amount;
                break;
            case Stats.DashCost:
                dashCost += amount;
                break;
            case Stats.MaxFuel:
                maxFuel += amount;
                break;
            case Stats.FuelSteal:
                fuelSteal += amount;
                break;
            case Stats.DrillRadius:
                drillRadius += amount;
                break;
            case Stats.DrillAttackSpeed:
                drillAttackSpeed += amount;
                break;
            case Stats.DrillDamage:
                drillDamage += amount;
                break;
            case Stats.CollectionRange:
                collectionRange += amount;
                break;
            default:
                Debug.LogWarning("Stat does not exist");
                break;
        }

        
    }

    public void InitializeAllStats()
    {
        Fuel fuel = GetComponent<Fuel>();
        PlayerMovement movement = GetComponent<PlayerMovement>();
        Drill drill = GetComponentInChildren<Drill>();
        CollectionRange range = GetComponentInChildren<CollectionRange>();

        fuel.InitializeStats();
        movement.InitializeStats();
        drill.InitializeStats();
        range.InitializeStats();
    }
}
