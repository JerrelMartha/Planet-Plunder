using UnityEngine;
using UnityEngine.InputSystem;

public partial class OpenClose : MonoBehaviour
{
    [SerializeField] private GameObject uiToToggle;
    [SerializeField] private bool isActive = false;

    // This allows you to bind any key/button in the Inspector
    [SerializeField] private InputAction toggleAction;

    private void OnEnable()
    {
        toggleAction.Enable();
    }

    private void OnDisable()
    {
        toggleAction.Disable();
    }

    private void Start()
    {
        // Set initial state
        uiToToggle.SetActive(isActive);
    }

    private void Update()
    {
        // Check if the action was performed this frame
        if (toggleAction.WasPressedThisFrame())
        {
            ToggleUI();
        }
    }

    public void ToggleUI()
    {
        isActive = !isActive;
        uiToToggle.SetActive(isActive);
    }
}