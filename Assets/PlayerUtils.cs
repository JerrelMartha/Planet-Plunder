using UnityEngine;

public class PlayerUtils : MonoBehaviour
{
    public static PlayerUtils Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform GetPlayerTransform()
    {
        return transform;
    }

    /// <summary>
    /// Returns the normalized direction from a chosen position to the player's position.
    /// </summary>
    public Vector2 GetDirectionToPlayer(Vector2 origin)
    {
        Vector2 playerPos = transform.position;
        return (playerPos - origin).normalized;
    }

    /// <summary>
    /// Returns the distance between an object and the player.
    /// </summary>
    public float GetDistanceToPlayer(Vector2 origin)
    {
        return Vector2.Distance(transform.position, origin);
    }
}