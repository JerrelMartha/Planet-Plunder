using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject target;

    [Header("Camera Settings")]
    [SerializeField] private float offsetZ = -1;
    [SerializeField] private float followEasing = 2f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomEasing = 2f;
    [SerializeField] private float zoomStrength = 1f;
    [Space]
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 20f;
    private float zoomTarget;

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;

    private Coroutine coroutine = null;

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        target = gameObject;
    }
    private void Update()
    {
        Vector2 scrollDelta = Mouse.current.scroll.ReadValue();

        if (scrollDelta.y > 0)
        {
            PlayerCursor.instance.ChangeSprite(PlayerCursor.Cursors.ZoomIn);

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(PlayerCursor.instance.DefaultCursorCooldown(0.4f));
        }

        if (scrollDelta.y < 0)
        {
            PlayerCursor.instance.ChangeSprite(PlayerCursor.Cursors.ZoomOut);

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(PlayerCursor.instance.DefaultCursorCooldown(0.4f));
        }

    }

    private void LateUpdate()
    {
        Camera cam = Camera.main;

        float zoomFactor = 0.1f / cam.orthographicSize * 10;

        float targetEasing = followEasing * zoomFactor * playerMovement.GetMovementSpeed();
        Vector2 scrollDelta = Mouse.current.scroll.ReadValue();

        // Follow player
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, offsetZ);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, targetEasing * Time.deltaTime);

        // Zoom
        zoomTarget -= scrollDelta.y * zoomStrength;
        zoomTarget = Mathf.Clamp(zoomTarget, minZoom, maxZoom);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, zoomEasing * Time.deltaTime);

    }

}
