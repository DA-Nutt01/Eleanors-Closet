using System;
using EC;
using UnityEngine;
using UnityEngine.AI;

public class InteractableSelectedState : IPlayerState
{
    private PlayerController m_PlayerController;
    private NavMeshAgent m_PlayerAgent;
    private BaseInteractable m_TargetInteractable;
    private ClickType m_ClickType;

    // Constructor
    public InteractableSelectedState(PlayerController playerController, BaseInteractable targetInteractable, ClickType clickType)
    {
        m_PlayerController = playerController;
        m_PlayerAgent = m_PlayerController.GetNavMeshAgent();
        m_TargetInteractable = targetInteractable;
        m_ClickType = clickType;
    }
    public void Enter()
    {
        // code that runs when we first enter the state
        Debug.Log($"Interactable Selected: {m_TargetInteractable}");
        // Subscribe to left click event
        InputManager.Instance.OnPlayerLeftMouseClick += InputManager_OnLeftMouseClick;
        // Subscribe to right click event
        InputManager.Instance.OnPlayerRightMouseClick += InputManager_OnRightClick;

        // Player sets destination to target interaractbles interaction zone
        SetTargetInteractable(m_TargetInteractable);
        Debug.Log("Entering Interactable Selected State");
    }

    public void Update()
    {
        switch (m_ClickType)
        {
            case ClickType.LeftClick:
                HandleLeftClickLogic();
                break;
            case ClickType.RightClick:
                HandleRightClickLogic();
                break;
            default:
                break;
        }
    }

    private void HandleLeftClickLogic()
    {
        // Overlap check to see if player is within interaction range of interactable
        Collider[] colliderArray = Physics.OverlapSphere(m_TargetInteractable.GetInteractionZone().position, m_TargetInteractable.GetInteractionRadius(), m_PlayerController.GetLayerMask());

        // Check if any colliders were collected and terminate early if not
        if (colliderArray.Length == 0) return;

        foreach (Collider collider in colliderArray)
        {
            //Debug.Log($"Collider in collider array: {collider.gameObject.name}");
            // Check if found collider has Player Controller component and is therefore the player
            if (collider.TryGetComponent(out PlayerController playerController))
            {
                // Transition to interacting state
                Debug.Log("Player Controller in range, interacting now.");
                // stop movement
                m_PlayerAgent.isStopped = true;
                m_PlayerAgent.ResetPath();
                m_PlayerAgent.velocity = Vector3.zero;

                // Trigger interaction
                m_TargetInteractable.Interact();

                // Transition back to idle or other state
                m_PlayerController.m_PlayerStateMachine.TryTransitionToState(new IdleState(m_PlayerController));
            }
            else Debug.Log("Player Controller not found");
        }
    }
    
    private void HandleRightClickLogic()
    {
        // Overlap check to see if player is within interaction range of interactable
        Collider[] colliderArray = Physics.OverlapSphere(m_TargetInteractable.GetInteractionZone().position, m_TargetInteractable.GetInteractionRadius(), m_PlayerController.GetLayerMask());

        // Check if any colliders were collected and terminate early if not
        if (colliderArray.Length == 0) return;

        foreach (Collider collider in colliderArray)
        {

            //Debug.Log($"Collider in collider array: {collider.gameObject.name}");

            // Check if found collider has Player Controller component and is therefore the player
            if (collider.TryGetComponent(out PlayerController playerController))
            {
                // Transition to interacting state
                Debug.Log("Player Controller in range, interacting now.");
                // stop movement
                m_PlayerAgent.isStopped = true;
                m_PlayerAgent.ResetPath();
                m_PlayerAgent.velocity = Vector3.zero;
                //Check for key item
                if (m_TargetInteractable.PlayerHasKey())
                {
                    // Toggle Locked State on Interactable
                    m_TargetInteractable.ToggleLockState();
                }
                else
                {
                    // Try to interact with the interactable
                    m_TargetInteractable.Interact();
                }
                // Transition back to idle or other state
                m_PlayerController.m_PlayerStateMachine.TryTransitionToState(new IdleState(m_PlayerController));
            }
            else Debug.Log("Player Controller not found");
        }
    }

    private void InputManager_OnLeftMouseClick(object sender, RaycastHitEventArgs args)
    {
        BaseInteractable interactable = args.Hit.collider.GetComponentInParent<BaseInteractable>();

        // If the object is an interactable, change state
        if (interactable != null)
        {
            SetTargetInteractable(interactable);
            m_ClickType = ClickType.LeftClick;
        }
        else
        {
            //If raycast did not hit an interactable, simply move the player to the position
            // Cache the coords of the ray
            Vector3 targetDestination = args.Hit.point;

            // Transition to Moving State
            m_PlayerController.m_PlayerStateMachine.TryTransitionToState(new MovingState(m_PlayerController, targetDestination));
        }
    }

    private void InputManager_OnRightClick(object sender, RaycastHitEventArgs args)
    {
        BaseInteractable interactable = args.Hit.collider.GetComponentInParent<BaseInteractable>();

        // If the object is an interactable, change state
        if (interactable != null)
        {
            SetTargetInteractable(interactable);
            m_ClickType = ClickType.RightClick;
        }
    }
    
    private void SetTargetInteractable(BaseInteractable newTarget)
    {
        m_TargetInteractable = newTarget;

        Vector3 targetPos = m_TargetInteractable.GetInteractionZone().position;

        m_PlayerAgent.SetDestination(targetPos);
    }

    public void Exit()
    {
        // code that runs when we exit the state
        // Stop the agent from moving
        m_PlayerAgent.isStopped = true;
        m_PlayerAgent.ResetPath();
        m_PlayerAgent.velocity = Vector3.zero;

        InputManager.Instance.OnPlayerLeftMouseClick -= InputManager_OnLeftMouseClick;
        InputManager.Instance.OnPlayerRightMouseClick -= InputManager_OnRightClick;

        Debug.Log("Exiting Interactable Selected State");
    }
}
