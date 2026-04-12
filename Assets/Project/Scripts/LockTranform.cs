using UnityEngine;

public class LockTransform : MonoBehaviour
{
    [SerializeField] private bool lockPosition = true;
    [SerializeField] private bool lockRotation = true;

    private RectTransform rectTransform;
    private Quaternion initialWorldRotation;
    private Vector2 initialAnchoredPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialWorldRotation = rectTransform.rotation;
        initialAnchoredPosition = rectTransform.anchoredPosition;
    }

    void LateUpdate()
    {
        if (rectTransform.parent == null) return;

        if (lockRotation)
        {
            rectTransform.rotation = Quaternion.identity;
        }

        if (lockPosition)
        {
            rectTransform.anchoredPosition = initialAnchoredPosition;
        }
    }
}