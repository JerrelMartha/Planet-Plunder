using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRadius = 3f;
    public float damageAmount = 10f;

    [Header("Detection")]
    public LayerMask resourceLayer;

    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PerformRadiusAttack();
        }
    }

    void PerformRadiusAttack()
    {
        // 1. Get mouse position in pixels
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        // 2. Convert pixels to World Space coordinates
        Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, _mainCamera.nearClipPlane));

        // Ensure the Z is correct for 2D physics checks (usually 0)
        Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        // 3. Use 2D OverlapCircle centered at the mouse
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mousePos2D, attackRadius, resourceLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<ResourceTile>(out ResourceTile tile))
            {
                tile.TakeDamage(damageAmount);
            }
        }
    }
}