using UnityEngine;
using UnityEngine.InputSystem;

public static class HelperFunctions
{
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

    public static string FormatNumber(float number)
    {
        if (number < 1000)
            return number.ToString("0.#");

        string[] suffixes = { "K", "M", "B" };
        int suffixIndex = -1;

        while (number >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000f;
            suffixIndex++;
        }

        return $"{number:0.#}{suffixes[suffixIndex]}";
    }
}