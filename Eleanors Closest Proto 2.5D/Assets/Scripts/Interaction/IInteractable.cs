using UnityEngine;

public interface IInteractable
{
    // Logic for handling what happens when this interactable is interacted with 
    void Interact();
    // Logic for computing if the player has the required item to (un)lock this interactable
    bool PlayerHasKey();
    // Logic for toggling this interactables state (Locked/Unlocked)
    void ToggleLockState();
    
}
