using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //static ref for singleton
    public static InputManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Camera m_ActiveCamera;
    [SerializeField] private InputActionAsset m_InputActions;

    // ACTIONS
    private InputAction m_LeftMouseClick;

    //EVENTS
    public event EventHandler<RaycastHitEventArgs> OnLeftMouseClick;

    private void OnEnable()
    {
        // Ensure m_ActiveCamera is assigned before enabling input actions
        if (m_ActiveCamera == null) m_ActiveCamera = Camera.main;
        
        // Enable the Player Action map
        m_InputActions.FindActionMap("Player").Enable();

    }

    private void OnDisable()
    {
        // Disable the Player Action Map
        m_InputActions.FindActionMap("Player").Disable();
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

        if (m_LeftMouseClick == null) m_LeftMouseClick = InputSystem.actions.FindAction("LeftMouseClick");

        // Subscribe Actions to Events
        m_LeftMouseClick.performed += OnLeftMouseClickPerformed;
    }

    private void OnLeftMouseClickPerformed(InputAction.CallbackContext cxt)
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
            OnLeftMouseClick?.Invoke(this, new RaycastHitEventArgs(hit));
        }
        else
        {
            Debug.Log("No hit detected.");
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
