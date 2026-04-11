using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private Boss bossScript;

    private void Update()
    {
        healthFill.fillAmount = bossScript.GetHealthNormalized();
        Debug.Log(bossScript.GetHealthNormalized());
    }
}
