using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleUI : MonoBehaviour
{
    [SerializeField] private GameObject toggledUI;

    private bool activated = false;
    [SerializeField] private InputAction hotkey;

    private void OnEnable()
    {
        hotkey.Enable();
    }

    private void OnDisable()
    {
        hotkey.Disable();
    }

    private void Update()
    {
        if (hotkey.WasPressedThisFrame())
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        activated = !activated;
        toggledUI.SetActive(activated);
    }
}