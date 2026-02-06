using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveVelocity;
    [Space]
    [SerializeField] private float boostMultiplier = 2f;
    [Space]
    [SerializeField] private float dashForce = 4f;
    [SerializeField] private float dashCooldown = 2f;

    [Header("References")]
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 lookDirection;

    [Header("Booleans")]
    private bool isBoosting = false;
    private bool canDash = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveVelocity = Mathf.Round(rb.linearVelocity.magnitude);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        lookDirection = (mousePos - (Vector2)transform.position).normalized;
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

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canDash)
        {
            StartCoroutine(Dash());
        }

    }

    private IEnumerator Dash()
    {
        rb.linearVelocity = (lookDirection * moveSpeed * dashForce);

        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public float GetMovementSpeed()
    {
        return moveSpeed;
    }

    public float GetVelocity()
    {
        return rb.linearVelocity.magnitude;
    }
}
