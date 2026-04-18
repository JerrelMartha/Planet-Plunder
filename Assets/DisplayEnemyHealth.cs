using UnityEngine;
using UnityEngine.UI;

public class DisplayEnemyHealth : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    private Enemy _enemy;

    void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (_enemy != null && healthBarFill != null)
        {
            healthBarFill.fillAmount = _enemy.GetHealthNormalized();
        }
    }
}