using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    /* 
    Single Responsibility: This script is responsible for managing the state machien for the player character
    */

    [Header("Components")]
    [SerializeField] private NavMeshAgent m_Agent;

    [Header("Settings")]
    [SerializeField] private float m_AgentSpeed = 3.5f;

    [Tooltip("The layer the player is on. Assign in Editor.")]
    [SerializeField] private LayerMask m_PlayerLayerMask;

    public PlayerStateMachine m_PlayerStateMachine { get; private set; }

    private void Awake()
    {
        // Instantiate State Machine
        m_PlayerStateMachine = new PlayerStateMachine(this);
        // Initialize State Machine state to Idle
        m_PlayerStateMachine.Initiaize(new IdleState(this));

        if (m_Agent == null) m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.speed = m_AgentSpeed;
    }
    private void Update()
    {
        m_PlayerStateMachine.Update();
    }
    public NavMeshAgent GetNavMeshAgent()
    {
        return m_Agent;
    }

    public LayerMask GetLayerMask()
    {
        return m_PlayerLayerMask;
    }
}


