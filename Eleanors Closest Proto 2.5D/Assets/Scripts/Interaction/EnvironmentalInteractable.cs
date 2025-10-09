using UnityEngine;
using EC;

public class EnvironmentalInteractable : BaseInteractable
{
    [Header("Environmental Interactable Configuration"), Space(5)]
    [Tooltip("Env data asset. Used to initialize certain class fields.")]
    [SerializeField] private EnvInteractableData m_EnvInteractableData;

    private ItemStorage m_ItemStorageComponent;

    protected override void Awake()
    {
        m_BaseInteractableData = m_EnvInteractableData;
        base.Awake();
    }
    public override void Interact()
    {
        // If this interactable currently has a stored item
        if (TryGetStoredItemData(out var itemData))
        {
            HandleInteractionByItemPrompt(itemData);
        }
        else
        {
            HandleInteractionByState();
        }
    }

    private bool TryGetStoredItemData(out BaseItemData itemData)
    {
        // Check if this interactable can store items by seeing if it has a ItemStorage component
        if (TryGetComponent<ItemStorage>(out var itemStorage))
        {
            itemData = itemStorage.GetStoredItemData();
            // Check if ItemStorage has an item stored
            if (itemData != null)
            {
                return itemData;
            }
        }

        itemData = null;
        return false;
    }

    private void HandleInteractionByItemPrompt(BaseItemData itemData)
    {
        // Tell Input Manager to Switch Input Action to UI, As locked UI will now be displayed
        InputManager.Instance.SwitchInputContext(InputContext.UIInteraction);
        // Tell UI Interaction Text Controller to prompt user to pick up item
        UIInteractionTextController.Instance.DisplayItemPickupPrompt(itemData);
    }
    
    private void HandleInteractionByState()
    {
        Debug.Log($"{m_State}");
        // Check the state of the interactable
        switch (m_State)
            {
            case InteractableState.Locked:
                //Check if key item is in player inventory to unlock this interactable
                // Tell Input Manager to Switch Input Action to UI, As locked UI will now be displayed
                InputManager.Instance.SwitchInputContext(InputContext.UIInteraction);
                // Tell UI Interaction Text Controller to display lock text list
                UIInteractionTextController.Instance.ShowLines(m_LockedInteractionTextList);
                break;
            case InteractableState.Unlocked:
                // Tell Input Manager to Switch Input Action to UI, As locked UI will now be displayed
                InputManager.Instance.SwitchInputContext(InputContext.UIInteraction);
                // Tell UI Interaction Text Controller to display lock text list
                UIInteractionTextController.Instance.ShowLines(m_UnlockedInteractionTextList);
                break;
            default:
                Debug.LogWarning($"No State Assigned To {this.gameObject.name}");
                break;
            }
    }
}
