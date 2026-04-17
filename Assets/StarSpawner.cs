using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float spawnDistance = 12f;
    [SerializeField] private float targetRadius = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnStar();
            timer = 0;
        }
    }

    void SpawnStar()
    {
        Vector2 spawnDir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = (Vector3)(spawnDir * spawnDistance);

        Vector2 roughCenterTarget = Random.insideUnitCircle * targetRadius;
        Vector3 moveDir = ((Vector3)roughCenterTarget - spawnPos).normalized;

        GameObject star = Instantiate(starPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = star.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speed = Random.Range(4f, 8f);
            rb.linearVelocity = (Vector2)moveDir * speed;
        }

        Destroy(star, 5f);
    }
}