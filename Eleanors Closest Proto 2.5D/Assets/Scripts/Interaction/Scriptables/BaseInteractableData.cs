using System.Collections.Generic;
using UnityEngine;
using EC;

public class BaseInteractableData : ScriptableObject
{
    // DO NOT INSTANTIATE
    // ALL Interactable scriptables inherit from this base one

    [Header("Base Interactable Configuration"), Space(5)]
    [Tooltip("The type of interactable this is.")]
    [SerializeField] protected string interactableName;
    [Tooltip("The type of interactable this is.")]
    [SerializeField] protected InteractableType type;
    [Tooltip("The state of this interactable. Determines the interaction text while interacting"), Space(5)]
    [SerializeField] protected InteractableState state;
    [Tooltip("A ref to a BaseItemData this interactable needs to be unlocked.")]
    [SerializeField] protected BaseItemData keyItem;

    [Tooltip("A list of text during interation while the interactable state is locked.")]
    [SerializeField] protected List<string> lockedInteractionTextList = new List<string>();

    [Tooltip("A list of text during interation while the interactable state is unlocked.")]
    [SerializeField] protected List<string> unlockedInteractionTextList = new List<string>();

    public List<string> GetLockedInteractionTextList()
    {
        return lockedInteractionTextList;
    }

    public List<string> GetUnlockedInteractionTextList()
    {
        return unlockedInteractionTextList;
    }

    public InteractableState GetState()
    {
        return state;
    }

    public BaseItemData GetKeyItem()
    {
        return keyItem;
    }

    public string GetName()
    {
        return interactableName;
    }
}
