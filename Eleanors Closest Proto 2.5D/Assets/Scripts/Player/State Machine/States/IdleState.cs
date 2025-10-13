using UnityEngine;
using UnityEngine.AI;
using EC;
public class IdleState : IPlayerState
{

    private PlayerController m_PlayerController;
    private NavMeshAgent m_PlayerAgent;
    // Constructor
    public IdleState(PlayerController playerController)
    {
        m_PlayerController = playerController;
        m_PlayerAgent = m_PlayerController.GetNavMeshAgent();
    }
    public void Enter()
    {
        // code that runs when we first enter the state

        // Subscribe to Input Manager Left  & Right click event
        InputManager.Instance.OnPlayerLeftMouseClick += InputManager_OnLeftMouseClick;
        InputManager.Instance.OnPlayerRightMouseClick += InputManager_OnRightMouseClick;

        Debug.Log("Entering Idle State");
    }

    private void InputManager_OnLeftMouseClick(object sender, RaycastHitEventArgs args)
    {
        BaseInteractable interactable = args.Hit.collider.GetComponentInParent<BaseInteractable>();

        // If the object is an interactable, change state
        if (interactable != null)
        {
            m_PlayerController.m_PlayerStateMachine.TryTransitionToState(new InteractableSelectedState(m_PlayerController, interactable, ClickType.LeftClick));
        }
        else
        {
            // If raycast did not hit an interactable, simply move the player to the position
            // Cache the coords of the ray
            Vector3 targetDestination = args.Hit.point;
            // Transition to moving state
            m_PlayerController.m_PlayerStateMachine.TryTransitionToState(new MovingState(m_PlayerController, targetDestination));
        }
    }

    private void InputManager_OnRightMouseClick(object sender, RaycastHitEventArgs args)
    {
        BaseInteractable interactable = args.Hit.collider.GetComponentInParent<BaseInteractable>();

         if (interactable != null)
        {
            m_PlayerController.m_PlayerStateMachine.TryTransitionToState(new InteractableSelectedState(m_PlayerController, interactable, ClickType.RightClick));
        }
    }

    public void Update()
    {
        // per-frame logic, include condition to transition to a new state
        // Check for left click input and check if an interactble or enviroment was hit
    }

    public void Exit()
    {
        // code that runs when we exit the state

        // Unsubscribe from events
        InputManager.Instance.OnPlayerLeftMouseClick -= InputManager_OnLeftMouseClick;
        InputManager.Instance.OnPlayerRightMouseClick -= InputManager_OnRightMouseClick;
        Debug.Log("Exiting Idle State");
    }
}
