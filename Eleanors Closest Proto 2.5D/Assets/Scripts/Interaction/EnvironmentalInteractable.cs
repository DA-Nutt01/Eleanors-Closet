using UnityEngine;
using EC;

public class EnvironmentalInteractable : BaseInteractable
{
    [Header("Environmental Interactable Configuration"), Space(5)]
    [Tooltip("Env data asset. Used to initialize certain class fields.")]
    [SerializeField] private EnvInteractableData m_EnvInteractableData;

    protected override void Awake()
    {
        m_BaseInteractableData = m_EnvInteractableData;
        base.Awake();
    } 
    public override void Interact()
    {
        Debug.Log($"{m_State}");
        // Check the state of the interactable
        switch (m_State)
        {
            case InteractableState.Locked:
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
