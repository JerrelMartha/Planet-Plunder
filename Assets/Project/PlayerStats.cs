using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

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
    MissileDamage,
    MissileAttackSpeed,
    MissileBulletSpeed,
    MissileArea,
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

    [Header("Missile")]
    public float missileDamage = 10f;
    public float missileAttackSpeed = 0.2f;
    public float missileBulletSpeed = 5f;
    public float missileArea = 3f;

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

    }

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Debug.Log("Manual Load Triggered via New Input System...");
            SaveSystem.LoadGame();

            // Log the value immediately to see if it changed
            Debug.Log($"Current moveSpeed: {moveSpeed}");
        }
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
            case Stats.MissileDamage:
                missileDamage += amount;
                break;
            case Stats.MissileAttackSpeed:
                missileAttackSpeed += amount;
                break;
            case Stats.MissileBulletSpeed:
                missileBulletSpeed += amount;
                break;
            case Stats.MissileArea:
                missileArea += amount;
                break;
            default:
                Debug.LogWarning("Stat does not exist");
                break;
        }

        SaveSystem.SaveGame();
    }
    

    public void SaveData(ref StatSaveData data)
    {
        data.moveSpeed = moveSpeed;
        data.dashForce = dashForce;
        data.boostMult = boostMultiplier;
        data.dashCooldown = dashCooldown;
        data.dashCost = dashCost;
        data.maxFuel = maxFuel;
        data.fuelSteal = fuelSteal;
        data.drillRadius = drillRadius;
        data.drillAttackSpeed = drillAttackSpeed;
        data.drillDamage = drillDamage;
        data.missileDamage = missileDamage;
        data.missileAttackSpeed = missileAttackSpeed;
        data.missileBulletSpeed = missileBulletSpeed;
        data.missileArea = missileArea;
        data.collectionRange = collectionRange;
    }

    public void LoadData(StatSaveData data)
    {
        this.moveSpeed = data.moveSpeed;
        this.dashForce = data.dashForce;
        this.boostMultiplier = data.boostMult;
        this.dashCooldown = data.dashCooldown;
        this.dashCost = data.dashCost;
        this.maxFuel = data.maxFuel;
        this.fuelSteal = data.fuelSteal;
        this.drillRadius = data.drillRadius;
        this.drillAttackSpeed = data.drillAttackSpeed;
        this.drillDamage = data.drillDamage;
        this.missileDamage = data.missileDamage;
        this.missileAttackSpeed = data.missileAttackSpeed;
        this.missileBulletSpeed = data.missileBulletSpeed;
        this.missileArea = data.missileArea;
        this.collectionRange = data.collectionRange;
    }

    [System.Serializable]
    public struct StatSaveData
    {
        public float moveSpeed, dashForce, boostMult, dashCooldown, dashCost;
        public float maxFuel, fuelSteal;
        public float drillRadius, drillAttackSpeed, drillDamage;
        public float missileDamage, missileAttackSpeed, missileBulletSpeed, missileArea;
        public float collectionRange;
    }
}
