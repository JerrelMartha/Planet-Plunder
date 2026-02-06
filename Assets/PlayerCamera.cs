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

    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        target = gameObject;

    }

    private void LateUpdate()
    {
        Camera cam = Camera.main;
        Vector2 scrollDelta = Mouse.current.scroll.ReadValue();

        float targetEasing = followEasing * 0.2f * playerMovement.GetMovementSpeed();

        // Follow player
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, offsetZ);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, targetEasing * Time.deltaTime);

        // Zoom
        zoomTarget -= scrollDelta.y * zoomStrength;
        zoomTarget = Mathf.Clamp(zoomTarget, minZoom, maxZoom);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, zoomEasing * Time.deltaTime);
    }

}
