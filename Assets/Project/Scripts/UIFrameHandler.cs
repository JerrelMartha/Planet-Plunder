using UnityEngine;
using UnityEngine.InputSystem;

public class UIFrameHandler : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 3.0f;
    [SerializeField] private float zoomSmoothing = 10f;

    [Header("Pan Settings")]
    [SerializeField] private float panSmoothing = 15f;

    private RectTransform rectTransform;
    private Vector3 targetScale;
    private Vector2 targetPosition;
    private Vector2 lastMousePosition;
    private bool isPanning;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        targetScale = rectTransform.localScale;
        targetPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        HandleInput();
        ApplySmoothing();
    }

    private void HandleInput()
    {
        bool panHeld = Mouse.current.middleButton.isPressed || Mouse.current.rightButton.isPressed;

        if (panHeld)
        {
            Vector2 currentMousePos = Mouse.current.position.ReadValue();

            if (!isPanning)
            {
                isPanning = true;
                lastMousePosition = currentMousePos;
            }

            Vector2 mouseDelta = currentMousePos - lastMousePosition;
            targetPosition += mouseDelta;
            lastMousePosition = currentMousePos;
        }
        else
        {
            isPanning = false;
        }

        float scrollValue = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scrollValue) > 0.01f)
        {
            float zoomIncrement = (scrollValue > 0 ? 1 : -1) * zoomSpeed;
            targetScale += Vector3.one * zoomIncrement;

            targetScale.x = Mathf.Clamp(targetScale.x, minZoom, maxZoom);
            targetScale.y = Mathf.Clamp(targetScale.y, minZoom, maxZoom);
            targetScale.z = 1f;
        }
    }

    private void ApplySmoothing()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            targetPosition,
            Time.deltaTime * panSmoothing
        );

        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            targetScale,
            Time.deltaTime * zoomSmoothing
        );
    }
}