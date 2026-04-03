using UnityEngine;
using UnityEngine.InputSystem;

public class HelperFunctions : MonoBehaviour
{
    public static HelperFunctions instance;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Converts the current mouse position to a Vector2 in World Space.
    /// </summary>
    public static Vector2 GetMouseWorldPosition()
    {
        if (Mouse.current == null) return Vector2.zero;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // For 2D, we use the camera's near clip plane or a fixed offset
        Vector3 mousePositionWithDepth = new Vector3(mouseScreenPos.x, mouseScreenPos.y, -Camera.main.transform.position.z);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePositionWithDepth);

        return new Vector2(worldPos.x, worldPos.y);
    }

    public static void SpawnParticleSystem(GameObject particles, float lifetime, Vector3 position)
    {
        GameObject particle = Instantiate(particles, position, Quaternion.identity);
        Destroy(particle, lifetime);
    }
}