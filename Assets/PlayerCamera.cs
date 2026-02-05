using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject target;

    [SerializeField] private float offsetZ = -1;
    [SerializeField] private float easing = 2f;

    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        target = gameObject;

    }

    private void LateUpdate()
    {
        float currentEasing = easing * 0.2f * playerMovement.GetMovementSpeed();
        // Lerp camera to player's position
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, offsetZ);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, currentEasing * Time.deltaTime);
    }
}
