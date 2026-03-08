using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fuel : MonoBehaviour
{
    public static Fuel instance;

    [SerializeField] private float currentFuel;
    [SerializeField] private float maxFuel;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            playerMovement = GetComponent<PlayerMovement>();
            DontDestroyOnLoad(gameObject);
        } else
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
            currentFuel -= 1.5f * Time.deltaTime;
        } else
        {
            currentFuel -= 1 * Time.deltaTime;
        }

        if (currentFuel < 0)
        {
            OnOutOfFuel();
        }
    }

    public void RemoveFuel(float amount)
    {
        currentFuel -= amount;
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
    }

    public float GetFuelNormalized()
    {
        return currentFuel / maxFuel;
    }

    public float GetFuel()
    {
        return currentFuel;
    }

    private void InitializeStats()
    {
        maxFuel = PlayerStats.instance.maxFuel;
        currentFuel = maxFuel;
    }

    private void OnOutOfFuel()
    {
        SceneManager.LoadScene("HomeBase");
    }
}
