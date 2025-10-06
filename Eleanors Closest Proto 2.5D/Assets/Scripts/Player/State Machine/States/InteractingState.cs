using UnityEngine;

public class InteractingState : IPlayerState
{
    private PlayerController m_PlayerController;
    private BaseInteractable m_TargetInteractable;

    // Constructor
    public InteractingState(PlayerController playerController, BaseInteractable targetInteratable)
    {
        m_PlayerController = playerController;
        m_TargetInteractable = targetInteratable;
    }
    public void Enter()
    {
        // code that runs when we first enter the state
        Debug.Log("Entering Interacting State");
        m_TargetInteractable.Interact();
        m_PlayerController.m_PlayerStateMachine.TryTransitionToState(new IdleState(m_PlayerController));
    }

    public void Update()
    {
        // per-frame logic, include condition to transition to a new state
        // State changes when left clicking on interactable
    }

    public void Exit()
    {
        // code that runs when we exit the state
	    Debug.Log("Exiting Interacting State");
    }
}
