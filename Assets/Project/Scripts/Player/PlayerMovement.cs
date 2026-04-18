using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float boostMultiplier;
    [SerializeField] private float dashForce = 4f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashCost = 1f;

    [Header("References")]
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    [SerializeField] private ParticleSystem boostPrt;

    [Header("Booleans")]
    private bool isBoosting = false;
    private bool canDash = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        InitializeStats();

        if (boostPrt != null)
        {
            boostPrt.Stop();
        }

        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.Stop();
        }
    }

    private void Update()
    {
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
        if (ctx.performed)
        {
            isBoosting = true;
            if (boostPrt != null) boostPrt.Play();
            if (audioSource != null && !audioSource.isPlaying) audioSource.Play();
        }
        else if (ctx.canceled)
        {
            isBoosting = false;
            if (boostPrt != null) boostPrt.Stop();
            if (audioSource != null) audioSource.Stop();
        }
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
        Fuel.instance.RemoveFuel(dashCost);
        SoundManager.Instance.PlaySound("Dash", true);

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

    public bool IsBoosting()
    {
        return isBoosting;
    }

    public void InitializeStats()
    {
        moveSpeed = PlayerStats.instance.moveSpeed;
        boostMultiplier = PlayerStats.instance.boostMultiplier;
        dashForce = PlayerStats.instance.dashForce;
        dashCost = PlayerStats.instance.dashCost;
        dashCooldown = PlayerStats.instance.dashCooldown;
    }
}