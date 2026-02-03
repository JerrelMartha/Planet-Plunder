using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boostMultiplier = 2f;
    [SerializeField] private float moveVelocity;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isBoosting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveVelocity = Mathf.Round(rb.linearVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        float speedToAdd = isBoosting ? moveSpeed * boostMultiplier : moveSpeed;

        rb.AddForce(moveDirection * speedToAdd);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext ctx)
    {
        isBoosting = ctx.performed;
    }
}
