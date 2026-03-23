using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuntimeUIManager : MonoBehaviour
{
    [Header("Fuel")]
    [SerializeField] private Image fuelFill;
    [SerializeField] private TextMeshProUGUI fuelCounter;
    [SerializeField] private float lerpSpeed = 5f;

    private void Update()
    {
        UpdateBars();
    }

    private void UpdateBars()
    {
        fuelFill.fillAmount = Mathf.Lerp(fuelFill.fillAmount, Fuel.instance.GetFuelNormalized(), Time.deltaTime * lerpSpeed);
        fuelCounter.text = Mathf.RoundToInt(Fuel.instance.GetFuel()).ToString();
    }
}