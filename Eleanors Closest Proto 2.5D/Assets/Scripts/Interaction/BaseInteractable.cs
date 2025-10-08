using System.Collections.Generic;
using UnityEngine;
using EC;

[System.Serializable]
public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [Header("Base Interactable Configuration"), Space(5)]

    protected BaseInteractableData m_BaseInteractableData;

    [Tooltip("The zone within which the player can interact with this object. Also the spawn point of the player when enering doors.")]
    [SerializeField] protected Transform m_InteractionZone;

    [Tooltip("The radius around this the player must be within to interact with this.")]
    [SerializeField] protected float m_InteractionRadius = 0.7f;

    [Tooltip("The enum of the state of the interactable")]
    [SerializeField] protected InteractableState m_State;
    
    [Tooltip("A list of text during interation while the interactable state is locked.")]
    [SerializeField] protected List<string> m_LockedInteractionTextList = new List<string>();
    
    [Tooltip("A list of text during interation while the interactable state is unlocked.")]
    [SerializeField] protected List<string> m_UnlockedInteractionTextList = new List<string>();

    protected virtual void Awake()
    {
        m_State = m_BaseInteractableData.GetState();
        m_LockedInteractionTextList = m_BaseInteractableData.GetLockedInteractionTextList();
        m_UnlockedInteractionTextList = m_BaseInteractableData.GetUnlockedInteractionTextList();
    }

    // Abstract Method Signatures (Implement in each respective Interactable)
    public abstract void Interact();

    // Concrete Methods
    public Transform GetInteractionZone()
    {
        return m_InteractionZone;
    }

    public float GetInteractionRadius()
    {
        return m_InteractionRadius;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (m_InteractionZone != null)
        {
            // Set the gizmo color (you can choose a color you like)
            Gizmos.color = Color.cyan;

            // Draw a wire sphere at the interaction zone position
            Gizmos.DrawWireSphere(m_InteractionZone.position, m_InteractionRadius);
        }
    }

}