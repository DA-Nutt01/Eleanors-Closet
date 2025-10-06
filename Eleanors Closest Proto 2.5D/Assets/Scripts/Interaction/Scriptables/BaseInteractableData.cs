using UnityEngine;

public class BaseInteractableData : ScriptableObject
{
    // DO NOT INSTANTIATE
    // ALL Interactable scriptables inherit from this base one
    private enum InteractableType
    {
        Item,
        Environment,
        Door,
    }

    private enum InteractableState
    {
        Locked,
        Unlocked,
    }

    [Header("Base Interactable Configuration"), Space(5)]
    public string interactionText;
    [SerializeField] private InteractableType type;

    [SerializeField] private InteractableState state;

}
