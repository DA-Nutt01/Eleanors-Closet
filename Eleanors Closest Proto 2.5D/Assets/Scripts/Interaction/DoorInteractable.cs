using EC;
using UnityEngine;

public class DoorInteractable : BaseInteractable
{
    [Header("Door Interactable Configuration"), Space(5)]
    [Tooltip("Door data asset that holds this door’s ID and target room enum")]
    [SerializeField] private DoorInteractableData m_DoorInteractableData;
    [Tooltip("The zone within which the player can interact with this object. Also the spawn point of the player when enering doors.")]
    [SerializeField] private Transform m_InteractionZoneB;

    [Header("Transition Settings"), Space(5)]
    [Tooltip("One of the two rooms this door connects (using RoomType enum). [Initializes itself from door data].")]
    [SerializeField] private RoomType m_RoomA;

    [Tooltip("One of the two rooms this door connects (using RoomType enum). [Initializes itself from door data].")]
    [SerializeField] private RoomType m_RoomB;

    protected override void Awake()
    {
        m_BaseInteractableData = m_DoorInteractableData;
        m_RoomA = m_DoorInteractableData.roomA;
        m_RoomB = m_DoorInteractableData.roomB;

        base.Awake();
    }

    public Transform GetConnectingRoomSpawnPoint(RoomType currentRoom)
    {
        if (currentRoom == m_RoomA) return m_InteractionZone;
        if (currentRoom == m_RoomB) return m_InteractionZoneB;

        Debug.LogError($"Room Spawn Point Not Found!");
        return null;
    }

    private RoomType GetConnectingRoomType(RoomType currentRoom)
    {
        if (currentRoom == m_RoomA) return m_RoomB;
        if (currentRoom == m_RoomB) return m_RoomA;
        Debug.LogWarning("Door isn't connected to given room");
        return RoomType.Null;
    }

    public override void Interact()
    {
        Debug.Log($"{m_State}");
        // Check the state of the interactable
        switch (m_State)
        {
            case InteractableState.Locked:
                foreach (string text in m_LockedInteractionTextList)
                {
                    Debug.Log($"{this.gameObject.name}: {text}");
                    // Tell Input Manager to Switch Input Action to UI, As locked UI will now be displayed
                    InputManager.Instance.SwitchInputContext(InputContext.UIInteraction);
                    // Tell UI Interaction Text Controller to display lock text list
                    UIInteractionTextController.Instance.ShowLines(m_LockedInteractionTextList);
                }
                break;
            case InteractableState.Unlocked:
                TryEnterRoom();
                break;
            default:
                Debug.LogWarning($"No State Assigned To {this.gameObject.name}");
                break;
        }

        // Grab the correct interaction text list based on the state
        // Ping another script to feed the text to the in game UI
    }

    private void TryEnterRoom()
    {
        //Debug.Log("Interacting with door: " + m_DoorInteractableData.interactionText);
        // Get the active room type (the room the player is in)
        RoomType activeRoom = RoomManager.Instance.GetActiveRoom().GetRoomType();

        // Get Connecting room type (the room the player is entering)
        RoomType connectingRoom = GetConnectingRoomType(activeRoom);

        // Find Room by room type
        Room roomToEnter = RoomManager.Instance.GetRoomFromEnum(connectingRoom);

        // Null check for room
        if (roomToEnter == null)
        {
            Debug.LogError("Target room not found: " + roomToEnter);
            return;
        }

        // Enter that room
        RoomManager.Instance.EnterRoom(roomToEnter, this);
    }
    
    protected override void OnDrawGizmosSelected()
    {
        if (m_InteractionZone != null && m_InteractionZoneB != null)
        {
            // Set the gizmo color (you can choose a color you like)
            Gizmos.color = Color.cyan;

            // Draw a wire sphere at the interaction zone position
            Gizmos.DrawWireSphere(m_InteractionZone.position, m_InteractionRadius);
            Gizmos.DrawWireSphere(m_InteractionZoneB.position, m_InteractionRadius);
        }
    }
}
