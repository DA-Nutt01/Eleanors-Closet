using System;
using UnityEngine;
using UnityEngine.InputSystem;
using EC;

public class InputManager : MonoBehaviour
{
    // Single Responsibility: This script handles raw player input and event firing

    //static ref for singleton
    public static InputManager Instance { get; private set; }

    [Header("Components"), Space(5)]
    [SerializeField] private Camera m_ActiveCamera;
    [SerializeField] private InputActionAsset m_InputActions;

    [Tooltip("Enum to track which set of controls are currently active bewteen the player and navigating UI.")]
    [SerializeField] private InputContext m_InputContext = InputContext.Gameplay;

    // PLAYER ACTIONS
    private InputAction m_PlayerLeftMouseClick;

    // UI Actions
    private InputAction m_UILeftMouseClick;

    // EVENTS
    public event EventHandler<RaycastHitEventArgs> OnPlayerLeftMouseClick;
    public event EventHandler OnUILeftMouseClick;

    private void OnEnable()
    {
        // Ensure m_ActiveCamera is assigned before enabling input actions
        if (m_ActiveCamera == null) m_ActiveCamera = Camera.main;

        // Enable the action map based on the current Input Context enum
        SwitchInputContext(m_InputContext);
    }

    private void OnDisable()
    {
        // Disable ALL Action Maps
        m_InputActions.Disable();
    }

    private void Awake()
    {
        // Check singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Cannot have more than one instance of InputManager");
            Destroy(gameObject);
            return;
        }

        // Initiaize Members

        // Initialize Player Actions & subscribe internal handler methods to the trigger of those actions
        if (m_PlayerLeftMouseClick == null) m_PlayerLeftMouseClick = m_InputActions.FindAction("Player/LeftMouseClick");
        if (m_PlayerLeftMouseClick != null) m_PlayerLeftMouseClick.performed += OnPlayerLeftMouseClickPerformed;

        // Initialize UI Actions
        if (m_UILeftMouseClick == null) m_UILeftMouseClick = m_InputActions.FindAction("UI/LeftMouseClick");
        if (m_UILeftMouseClick != null) m_UILeftMouseClick.performed += OnUILeftMouseClickPerformed;
        
    }

    private void OnPlayerLeftMouseClickPerformed(InputAction.CallbackContext cxt)
    {
        // Cache the mouses current pos
        Vector3 screenPos = Mouse.current.position.ReadValue();

        // Shoot a ray at the mouse pos
        Ray ray = m_ActiveCamera.ScreenPointToRay(screenPos);

        // Cache the raycasthit
        RaycastHit hit;

        // Check if the ray hit an object 
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.Log("Hit: " + hit.transform.name); // Verify hit detection
            // Fire the event & send raycasthit as args to listeners
            OnPlayerLeftMouseClick?.Invoke(this, new RaycastHitEventArgs(hit));
        }
        else
        {
            Debug.Log("No hit detected.");
        }
    }

    private void OnUILeftMouseClickPerformed(InputAction.CallbackContext cxt)
    {
        // UI Interaction Text Controller should subscribe to this event to know when to display the next line of text

        OnUILeftMouseClick?.Invoke(this, EventArgs.Empty);
    }

    public void SwitchInputContext(InputContext ctx)
    {
        // Takces in an Input context enum and switches to the given input context
        switch (ctx)
        {
            case InputContext.Gameplay:
                m_InputActions.FindActionMap("Player").Enable();
                m_InputActions.FindActionMap("UI").Disable();
                Debug.Log($"Switch Input Context from UI to Gameplay");
                break;
            case InputContext.UIInteraction:
                m_InputActions.FindActionMap("Player").Disable();
                m_InputActions.FindActionMap("UI").Enable();
                Debug.Log($"Switch Input Context from Gameplay to UI");
                break;
            default:
                Debug.LogWarning($"Input Context Not Found!");
                break;
        }
    }
}

public class RaycastHitEventArgs : EventArgs
{
    // Ref to RaycastHit
    public RaycastHit Hit { get; }

    // Constructor
    public RaycastHitEventArgs(RaycastHit hit)
    {
        Hit = hit;
    }
}
