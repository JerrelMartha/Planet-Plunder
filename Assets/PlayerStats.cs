using UnityEngine;

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

    }

    
}
