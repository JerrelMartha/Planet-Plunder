using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    private Camera _mainCamera;

    void Update()
    {
        AimAtMouse();
    }

    private void AimAtMouse()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldMousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -_mainCamera.transform.position.z));
        Vector2 direction = (Vector2)worldMousePosition - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}