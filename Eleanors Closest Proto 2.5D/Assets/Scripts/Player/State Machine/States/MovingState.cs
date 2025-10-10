using UnityEngine;
using UnityEngine.AI;

public class MovingState : IPlayerState
{
    // While moving, the player agent has a destination and is actively traveling towards it
    // Only in this state with a desitnation but not selected interactable
    // Transitions to Idle state when agent reaches destination
    // Transitions to interaction selected if one is selected during movement

    private PlayerController m_PlayerController;
    private NavMeshAgent m_PlayerAgent;
    private Vector3 m_Destination;

    //Constructor
    public MovingState(PlayerController playerController, Vector3 destination)
    {
        m_PlayerController = playerController; 
        m_PlayerAgent = m_PlayerController.GetNavMeshAgent();
        m_Destination = destination;
    }

    public void Enter()
    {
        Debug.Log("Entering Moving State");

        // Subscribe to Input Manager Left click event
        InputManager.Instance.OnPlayerLeftMouseClick += InputManager_OnLeftMouseClick;

        // Set the destination
        m_PlayerAgent.isStopped = false;
        m_PlayerAgent.SetDestination(m_Destination);
    }

    public void Update()
    {
        // Don’t do anything until path is ready
        if (m_PlayerAgent.pathPending)
        {
            Debug.LogWarning($"[Moving State] Agent path pending");
            return;
        }

        // Check if agent path is valid
        if (m_PlayerAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogWarning($"[Moving State] Agent path is invalid");
            return;
        }

        // Check if agent still has remaining distance to travel
        if (m_PlayerAgent.remainingDistance > m_PlayerAgent.stoppingDistance)
        {
            //Debug.LogWarning($"[Moving State] Agent has remaining travel distance");
            return;
        }

        // Check if path is complete
        if (!m_PlayerAgent.pathPending &&
            m_PlayerAgent.pathStatus == NavMeshPathStatus.PathComplete &&
            m_PlayerAgent.remainingDistance <= m_PlayerAgent.stoppingDistance)
        {
            // Path is complete, transition to idle state
            Debug.Log($"[Moving State] Path complete, transitioning to Idle State");
            m_PlayerController.m_PlayerStateMachine.TryTransitionToState(new IdleState(m_PlayerController));
        }
    }

    public void Exit()
    {
        // code that runs when we exit the state

        // Stop the agent from moving
        m_PlayerAgent.isStopped = true;
        m_PlayerAgent.ResetPath();
        m_PlayerAgent.velocity = Vector3.zero;

        // Unsubscribe from events
        InputManager.Instance.OnPlayerLeftMouseClick -= InputManager_OnLeftMouseClick;
        Debug.Log("Exiting Moving State");
    }

    private void InputManager_OnLeftMouseClick(object sender, RaycastHitEventArgs args)
    {
        BaseInteractable interactable = args.Hit.collider.GetComponentInParent<BaseInteractable>();

        // If the object is an interactable, change state
        if (interactable != null)
        {
            m_PlayerController.m_PlayerStateMachine.TryTransitionToState(new InteractableSelectedState(m_PlayerController, interactable));
        }
        else
        {
            // If raycast did not hit an interactable, simply move the player to the position

            // Update & Set new destination 
            m_Destination = args.Hit.point;
            m_PlayerAgent.SetDestination(m_Destination);
            Debug.Log($"[Moving State] Updating Destination to {m_Destination}");
            
        }
    }
}

