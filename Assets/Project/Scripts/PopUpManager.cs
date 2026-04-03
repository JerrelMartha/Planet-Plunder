using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager instance;

    [SerializeField] private GameObject popUp;
    [SerializeField] private PopUpSO[] popUps;

    private void Awake()
    {
        instance = this;
    }

    public void CollectPopUp(string resourceTag)
    {
        switch (resourceTag.ToLower())
        {
            case "dirt": CreatePopUp(popUps[0]); break;
            case "stone": CreatePopUp(popUps[1]); break;
            case "iron": CreatePopUp(popUps[2]); break;
            case "gold": CreatePopUp(popUps[3]); break;
            case "diamond": CreatePopUp(popUps[4]); break;

            default: Debug.LogWarning("Pop Up Not Found!"); break;
        }
    }

    private void CreatePopUp(PopUpSO PopUp)
    {
        GameObject message = Instantiate(popUp, transform.position, Quaternion.identity);
    }
}
