using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Resource 
{ 
    dirt,
    stone,
    iron,
    gold,
    diamond,
}


public class PlayerResources : MonoBehaviour
{
    public static PlayerResources instance;

    // Holds resource type and how much of that resource you have
    private Dictionary<Resource, float> resourceInventory = new Dictionary<Resource, float>();
    private Dictionary<Resource, float> temporaryInventory = new Dictionary<Resource, float>();


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

    private void Update()
    {
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            LogResources();
        }
    }

    public void AddResource(Resource resourceType, float amount)
    {
        // If resource is not yet in the Dictionary, initializes the resource to 0.
        if (!resourceInventory.ContainsKey(resourceType))
        {
            resourceInventory[resourceType] = 0;
        }
        resourceInventory[resourceType] += amount;
    }

    public void AddTemporaryResource(Resource resourceType, float amount)
    {
        // If resource is not yet in the Dictionary, initializes the resource to 0.
        if (!temporaryInventory.ContainsKey(resourceType))
        {
            temporaryInventory[resourceType] = 0;
        }
        temporaryInventory[resourceType] += amount;
    }

    public void ResetTemporaryInventory()
    {
        temporaryInventory = null;
    }

    public float GetResourceAmount(Resource type)
    {
        // If resource in not yet in the Dictionary, returns 0 (because you don't have that resource yet)
        return resourceInventory.GetValueOrDefault(type, 0f);
    }

    [ContextMenu("Log Resources")]
    public void LogResources()
    {
        foreach (var kvp in resourceInventory)
        {
            Debug.Log($"Resource: {kvp.Key}, Amount: {kvp.Value}");
        }
    }


}
