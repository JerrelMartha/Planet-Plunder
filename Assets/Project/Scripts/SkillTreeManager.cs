using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    [SerializeField] private GameObject skillTreeUI;

    private bool SkillTreeActive = false;

    private void Start()
    {
        
    }

    public void ToggleSkillTree()
    {
        SkillTreeActive = !SkillTreeActive;

        skillTreeUI.SetActive(SkillTreeActive);
    }
}
