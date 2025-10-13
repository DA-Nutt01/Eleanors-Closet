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
    [Tooltip("A layer mask to define what colliders are clickable for moving & interacting.")]
    [SerializeField] private LayerMask m_ClickableLayerMask;
    [Tooltip("A layer mask to define what colliders are interactable for interacting & using items.")]
    [SerializeField] private LayerMask m_InteractableLayerMask;

    // PLAYER ACTIONS
    private InputAction m_PlayerLeftMouseClick;
    private InputAction m_PlayerRightMouseClick;


    // UI Actions
    private InputAction m_UILeftMouseClick;

    // PLAYER EVENTS
    public event EventHandler<RaycastHitEventArgs> OnPlayerLeftMouseClick;
    public event EventHandler<RaycastHitEventArgs> OnPlayerRightMouseClick;
    // UI EVENTS
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

        if (m_PlayerRightMouseClick == null) m_PlayerRightMouseClick = m_InputActions.FindAction("Player/RightMouseClick");
        if (m_PlayerRightMouseClick != null) m_PlayerRightMouseClick.performed += OnPlayerRightMouseClickPerformed;

        // Initialize UI Actions
        if (m_UILeftMouseClick == null) m_UILeftMouseClick = m_InputActions.FindAction("UI/LeftMouseClick");
        if (m_UILeftMouseClick != null) m_UILeftMouseClick.performed += OnUILeftMouseClickPerformed;
        
    }

    private void OnPlayerLeftMouseClickPerformed(InputAction.CallbackContext cxt)
    {
        // Try to get a raycast hit from the mouse
        if (TryGetRaycastHitOnLayerMask(m_ClickableLayerMask, out RaycastHit hit))
        {
            // If it gets a hit, invoke the event with those raycast event args
            OnPlayerLeftMouseClick?.Invoke(this, new RaycastHitEventArgs(hit));
        }
    }

    private void OnPlayerRightMouseClickPerformed(InputAction.CallbackContext cxt)
    {
        if (TryGetRaycastHitOnLayerMask(m_InteractableLayerMask, out RaycastHit hit))
        {
            //Debug.Log("Interacrtable Clicked!");
            // If the ray gets a hit, invoke the event with the args
            OnPlayerRightMouseClick?.Invoke(this, new RaycastHitEventArgs(hit));
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

    private bool TryGetRaycastHitOnLayerMask(LayerMask layerMask, out RaycastHit hit)
    {
         // Cache the mouses current pos
        Vector3 screenPos = Mouse.current.position.ReadValue();

        // Shoot a ray at the mouse pos
        Ray ray = m_ActiveCamera.ScreenPointToRay(screenPos);

        // Check if the ray hit an object 
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //Debug.Log("Hit: " + hit.transform.name); // Verify hit detection
            return true;
        }
        else
        {
            //Debug.Log("No hit detected.");
            return false;
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
