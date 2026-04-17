using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private bool usePitchShift = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound("ButtonHover", usePitchShift);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound("ButtonClick", usePitchShift);
    }
}
