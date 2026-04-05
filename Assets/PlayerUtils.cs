using UnityEngine;

public class PlayerUtils : MonoBehaviour
{
    public static PlayerUtils instance;

    private void Start()
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

    public Transform GetPlayerTransform()
    {
        return transform;
    }

    /// <summary>
    /// Returns the normalized direction from chosen position to player's position.
    /// </summary>
    /// <param name="objPosition"></param>
    /// <returns></returns>
    public Vector2 GetDirectionToPlayer(Vector2 objPosition)
    {
        Vector2 playerPosition = transform.position;

        Vector2 direction = playerPosition - objPosition;

        return direction.normalized;
    }
}
