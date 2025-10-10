using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIInventoryManager : MonoBehaviour
{
    /*
    Single Responsibility: This script is responsible for updating the UI of the item the player has equipped at any given time
    */

    public static UIInventoryManager Instance { get; private set; }

    [Tooltip("A ref to the text object to display the currently held item of the player. Assign in Editor.")]
    [SerializeField] private TextMeshProUGUI m_InventoryText;

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
            Debug.LogWarning("Cannot have more than one instance of UI Inventory Manager");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        RefreshInventoryText();
    }

    public void RefreshInventoryText()
    {
        if (InventoryManager.Instance.GetStoredItemData() == null)
        {
            // Inventory is empty
            m_InventoryText.text = "Empty";
            return;
        }

        string text = InventoryManager.Instance.GetStoredItemData().GetName();
        m_InventoryText.text = text;
    }

}
