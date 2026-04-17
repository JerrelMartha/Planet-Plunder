using UnityEngine;
using UnityEngine.EventSystems;

public class Star : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject particles;
    private Rigidbody2D rb2d;

    void Start()
    {
        float randomScale = Random.Range(0.2f, 0.4f);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);

        rb2d = GetComponent<Rigidbody2D>();

        float randomForce = Random.Range(1f, 10f);
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        rb2d.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);

        Destroy(gameObject, 5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject spawned = Instantiate(particles, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound("Explode", true);
        Destroy(spawned, 2f);
        Destroy(gameObject);
    }
}