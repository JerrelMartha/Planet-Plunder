using UnityEngine;

public class HelperFunctions : MonoBehaviour
{
    public static HelperFunctions Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void SpawnParticleSystem(GameObject particles, float lifetime, Vector3 position)
    {
        GameObject particle = Instantiate(particles, position, Quaternion.identity);
        Destroy(particle, lifetime);
    }
}
