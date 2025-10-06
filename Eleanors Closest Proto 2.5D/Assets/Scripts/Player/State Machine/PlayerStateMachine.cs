using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine
{
    public IPlayerState CurrentState { get; private set; }
    private PlayerController m_PlayerController;

    // Event to notify other objects of state change
    public event Action<IPlayerState> onStateChange;

    // Constructor (this class must be instantiated as it is not a Monobeaviour)
    public PlayerStateMachine(PlayerController playerController)
    {
        m_PlayerController = playerController;
        // Create an instance of each state & pass in the controller
    }

    // Set starting state
    public void Initiaize(IPlayerState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();

        onStateChange?.Invoke(startingState);
    }

    public void TryTransitionToState(IPlayerState newState)
    {
        if (newState == null)
        {
            Debug.LogError("State Not Found!");
            return;
        }
        
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null) CurrentState.Update();
        else Debug.LogWarning("No active state!");
    }
}
