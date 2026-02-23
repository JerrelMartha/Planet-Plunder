using UnityEngine;
using UnityEngine.InputSystem; // Ensure you have the Input System package

public class PlayerAim : MonoBehaviour
{
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        AimAtMouse();
    }

    private void AimAtMouse()
    {
        // 1. Get the mouse position in screen space
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // 2. Convert screen position to world position
        Vector3 worldMousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -_mainCamera.transform.position.z));

        // 3. Calculate the direction from the object to the mouse
        Vector2 direction = (Vector2)worldMousePosition - (Vector2)transform.position;

        // 4. Calculate the angle in degrees
        // Use Atan2 to get the angle from the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 5. Apply the rotation
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}