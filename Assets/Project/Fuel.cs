using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fuel : MonoBehaviour
{
    public static Fuel instance;

    [SerializeField] private float currentFuel;
    [SerializeField] private float maxFuel;
    [SerializeField] private GameObject endScreen;

    private bool isGameOver = false;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            playerMovement = GetComponent<PlayerMovement>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeStats();
    }

    private void Update()
    {
        DecayFuelOvertime();
    }

    private void DecayFuelOvertime()
    {
        if (playerMovement.IsBoosting())
        {
            RemoveFuel(1.5f * Time.deltaTime);
        }
        else
        {
            RemoveFuel(1.5f * Time.deltaTime);
        }      
    }

    public void RemoveFuel(float amount)
    {
        // Prevent fuel removal if the game is already over
        if (isGameOver) return;

        currentFuel -= amount;

        if (currentFuel <= 0)
        {
            currentFuel = 0; // Clamp to 0
            OnOutOfFuel();
        }
    }

    private void OnOutOfFuel()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;
        Instantiate(endScreen);
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;

        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }

    public float GetFuelNormalized()
    {
        return currentFuel / maxFuel;
    }

    public float GetFuel()
    {
        return currentFuel;
    }

    public void InitializeStats()
    {
        maxFuel = PlayerStats.instance.maxFuel;
        currentFuel = maxFuel;
    }


}
