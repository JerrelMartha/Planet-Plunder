using UnityEngine;

public class OpenClose : MonoBehaviour
{
    [SerializeField] private GameObject uiToToggle;
    [SerializeField] private bool isActive = false;

    private void Start()
    {
        UpdateState();
    }
    public void UpdateState()
    {
        uiToToggle.SetActive(isActive);
        isActive = !isActive;
    }
}
