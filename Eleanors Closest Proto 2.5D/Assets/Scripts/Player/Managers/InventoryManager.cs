using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    /*
    Single Responsibility: This script is responsible for managing the inventory of the player
    - Store a single item scriptable 
    - Set item
    - Clear Item
    */

    public static InventoryManager Instance { get; private set; }

    [Tooltip("A ref to a BaseItem object the player is holdig. Inventory capaciy is 1 item at a time")]
    [SerializeField] private BaseItemData m_StoredItemData = null;

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

    public void TryTakeItemFromStorage(BaseItemData itemToGrab, ItemStorage storageFrom)
    {
        // Takes in an itemdata to set, and the ItemStorage its getting it from.
        // Sets that itemdata to storedItem
        // Null check newItem
        if (itemToGrab == null)
        {
            Debug.LogError($"ItemToGrab is missing.");
            return;
        }

        if (storageFrom == null)
        {
            Debug.LogError($"ItemStorage component is missing.");
            return;
        }

        // Check if player inventory is empty or not
        if (m_StoredItemData == null)
        {
            // Player inventory is empty, Set item to storedItem and Clear item in storageFrom
            Debug.Log($"Player taking {itemToGrab.name} from {storageFrom.gameObject.name}");
            // Set stored item for inventory
            m_StoredItemData = itemToGrab;
            // Clear item from interactable
            storageFrom.ClearStoredItemData();
            // Tell UIInventoryManager to update inventory text
            UIInventoryManager.Instance.RefreshInventoryText();
        }
        else if (m_StoredItemData != null)
        {
            // Player has an item already in hand, swap items from hand to storageFrom
            Debug.Log($"Player swapping {m_StoredItemData.name} with {storageFrom.gameObject.name}'s {itemToGrab.name}");
            BaseItemData currentItem = m_StoredItemData;
            m_StoredItemData = storageFrom.GetStoredItemData();
            storageFrom.SetStoredItemData(currentItem);
        }
    }
    
    public BaseItemData GetStoredItemData()
    {
        return m_StoredItemData;
    }
    
}
