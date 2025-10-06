using UnityEditor.EditorTools;
using UnityEngine;

[System.Serializable]
public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [Header("Base Interactable Configuration"), Space(5)]

    [Tooltip("The zone within which the player can interact with this object. Also the spawn point of the player when enering doors.")]
    [SerializeField] protected Transform m_InteractionZone;

    [Tooltip("The radius around this the player must be within to interact with this.")]
    [SerializeField] protected const float m_InteractionRadius = 0.7f;

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