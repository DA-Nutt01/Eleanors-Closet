using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    /*
    Single Responsibility: This script is responsible for managing the inventory of the player
    - Store a single item scriptable 
    - Examine action for the item
    - Use action for the item (How does the player initiate using items?)
    */

    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        // Check singleton
        if (Instance == null & Instance != this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Cannot have more than one instance of InventoryManger!");
            Destroy(gameObject);
            return;
        }
    } 

    
}
