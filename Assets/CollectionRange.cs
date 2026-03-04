using UnityEngine;

public class CollectionRange : MonoBehaviour
{
    [SerializeField] private float range = 1.5f;
    private LayerMask resourceLayer = 3;

    [SerializeField] private bool updateRangePerFrame = false;

    private void Start()
    {
        UpdateRange();
    }

    private void Update()
    {
        if (updateRangePerFrame)
        {
            UpdateRange();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == resourceLayer)
        {
            collision.gameObject.GetComponent<DroppedResource>().Collect();
        }
    }

    private void UpdateRange()
    {
        transform.localScale = new Vector3(range, range, 1);
    }
}
