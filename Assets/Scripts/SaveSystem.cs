using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public void SaveData()
    {
        
    }

    public void LoadData()
    {

    }
}

public class SaveData 
{ 

}

