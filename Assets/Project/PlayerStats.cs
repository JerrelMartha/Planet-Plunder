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
    MissileEnemyDamage,
    DrillEnemyDamage,
    MissileCost,
    // New Cluster Stats
    ClusterDamage,
    ClusterBombDamage,
    ClusterAmount,
    ClusterBombAttackSpeed
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
    public float drillEnemyDamage = 0.5f;

    [Header("Missile")]
    public float missileDamage = 10f;
    public float missileAttackSpeed = 0.2f;
    public float missileBulletSpeed = 5f;
    public float missileArea = 3f;
    public float missileEnemyDamage = 20f;
    public float missileCost = 2f;

    [Header("Cluster")]
    public float clusterDamage = 5f;
    public float clusterBombDamage = 8f;
    public float clusterAmount = 3f;
    public float clusterBombAttackSpeed = 1f;

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
            Debug.Log($"Current moveSpeed: {moveSpeed}");
        }
    }

    public void IncreaseStat(Stats stats, float amount)
    {
        switch (stats)
        {
            case Stats.MoveSpeed: moveSpeed += amount; break;
            case Stats.DashForce: dashForce += amount; break;
            case Stats.BoostMult: boostMultiplier += amount; break;
            case Stats.DashCooldown: dashCooldown += amount; break;
            case Stats.DashCost: dashCost += amount; break;
            case Stats.MaxFuel: maxFuel += amount; break;
            case Stats.FuelSteal: fuelSteal += amount; break;
            case Stats.DrillRadius: drillRadius += amount; break;
            case Stats.DrillAttackSpeed: drillAttackSpeed += amount; break;
            case Stats.DrillDamage: drillDamage += amount; break;
            case Stats.CollectionRange: collectionRange += amount; break;
            case Stats.MissileDamage: missileDamage += amount; break;
            case Stats.MissileAttackSpeed: missileAttackSpeed += amount; break;
            case Stats.MissileBulletSpeed: missileBulletSpeed += amount; break;
            case Stats.MissileArea: missileArea += amount; break;
            case Stats.DrillEnemyDamage: drillEnemyDamage += amount; break;
            case Stats.MissileEnemyDamage: missileEnemyDamage += amount; break;
            case Stats.MissileCost: missileCost += amount; break;
            // New Cluster Cases
            case Stats.ClusterDamage: clusterDamage += amount; break;
            case Stats.ClusterBombDamage: clusterBombDamage += amount; break;
            case Stats.ClusterAmount: clusterAmount += amount; break;
            case Stats.ClusterBombAttackSpeed: clusterBombAttackSpeed += amount; break;
            default:
                Debug.LogWarning("Stat does not exist");
                break;
        }

        SaveSystem.SaveGame();
    }

    public float GetStatValue(Stats stats)
    {
        return stats switch
        {
            Stats.MoveSpeed => moveSpeed,
            Stats.DashForce => dashForce,
            Stats.BoostMult => boostMultiplier,
            Stats.DashCooldown => dashCooldown,
            Stats.DashCost => dashCost,
            Stats.MaxFuel => maxFuel,
            Stats.FuelSteal => fuelSteal,
            Stats.DrillRadius => drillRadius,
            Stats.DrillAttackSpeed => drillAttackSpeed,
            Stats.DrillDamage => drillDamage,
            Stats.CollectionRange => collectionRange,
            Stats.MissileDamage => missileDamage,
            Stats.MissileAttackSpeed => missileAttackSpeed,
            Stats.MissileBulletSpeed => missileBulletSpeed,
            Stats.MissileArea => missileArea,
            Stats.DrillEnemyDamage => drillEnemyDamage,
            Stats.MissileEnemyDamage => missileEnemyDamage,
            Stats.MissileCost => missileCost,
            // New Cluster Cases
            Stats.ClusterDamage => clusterDamage,
            Stats.ClusterBombDamage => clusterBombDamage,
            Stats.ClusterAmount => clusterAmount,
            Stats.ClusterBombAttackSpeed => clusterBombAttackSpeed,
            _ => 0
        };
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
        data.missileCost = missileCost;
        // Save new stats
        data.clusterDamage = clusterDamage;
        data.clusterBombDamage = clusterBombDamage;
        data.clusterAmount = clusterAmount;
        data.clusterBombAttackSpeed = clusterBombAttackSpeed;
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
        this.missileCost = data.missileCost;
        // Load new stats
        this.clusterDamage = data.clusterDamage;
        this.clusterBombDamage = data.clusterBombDamage;
        this.clusterAmount = data.clusterAmount;
        this.clusterBombAttackSpeed = data.clusterBombAttackSpeed;
    }

    [System.Serializable]
    public struct StatSaveData
    {
        public float moveSpeed, dashForce, boostMult, dashCooldown, dashCost;
        public float maxFuel, fuelSteal;
        public float drillRadius, drillAttackSpeed, drillDamage;
        public float missileDamage, missileAttackSpeed, missileBulletSpeed, missileArea, missileCost;
        public float collectionRange;
        // New save fields
        public float clusterDamage, clusterBombDamage, clusterAmount, clusterBombAttackSpeed;
    }

    public void ResetToDefaults()
    {
        moveSpeed = 5f;
        dashForce = 2f;
        boostMultiplier = 1.5f;
        dashCooldown = 2f;
        dashCost = 1f;
        maxFuel = 20f;
        fuelSteal = 0f;
        drillRadius = 0.2f;
        drillAttackSpeed = 5f;
        drillDamage = 1f;
        drillEnemyDamage = 0.5f;
        missileDamage = 20f;
        missileAttackSpeed = 0.7f;
        missileBulletSpeed = 5f;
        missileArea = 1f;
        missileEnemyDamage = 20f;
        missileCost = 2f;
        clusterDamage = 20f;
        clusterBombDamage = 20f;
        clusterAmount = 3f;
        clusterBombAttackSpeed = 1f;
        collectionRange = 3f;
    }
}