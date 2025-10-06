using System;
using System.Collections.Generic;
using UnityEngine;
using EC;
using UnityEngine.AI;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }
    [Header("Room Configuration")]
    [Tooltip("All room instances in the scene — used to populate lookup table")]
    [SerializeField] private List<Room> m_AllRooms = new List<Room>();

    [Tooltip("Lookup dictionary from RoomType enum to Room instance (not serialized)")]
    private Dictionary<RoomType, Room> m_RoomLookup = new Dictionary<RoomType, Room>();

    [Header("Active Room & Player")]
    [Tooltip("The room currently considered 'active'")]
    [SerializeField] private Room m_ActiveRoom;

    [Tooltip("Reference to the Player NavMeshAgent Component, used to reposition when switching rooms")]
    [SerializeField] private GameObject m_Player;

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
            Debug.LogWarning("Cannot have more than one instance of Room Manager");
            Destroy(gameObject);
            return;
        }

        if (m_ActiveRoom == null) Debug.LogWarning("ActiveRoom on Awake: NULL");
    }

    private void Start()
    {
        // Disable all inactive rooms
        foreach (Room room in m_AllRooms)
        {
            if (room != m_ActiveRoom)
                DisableRoom(room);
        }

        // Enable active room
        EnableRoom(m_ActiveRoom);
    }

    public void RegisterRoom(Room room)
    {
        m_AllRooms.Add(room);
        m_RoomLookup.Add(room.GetRoomType(), room);
    }

    public Room GetRoomFromEnum(RoomType roomType)
    {
        if (m_RoomLookup.TryGetValue(roomType, out Room room)) return room;

        Debug.LogError("No room found for name " + roomType);
        return null;
    }

    public Room GetActiveRoom()
    {
        return m_ActiveRoom;
    }

    public void EnterRoom(Room roomToEnter, DoorInteractable connectingDoor)
    {
        Debug.Log($"EnterRoom called: entering {roomToEnter?.name}");

        // Null check for room to enter
        if (roomToEnter == null)
        {
            Debug.LogError("roomToEnter not found");
            return;
        }

        // Cache old room
        Room roomToLeave = m_ActiveRoom;

        // Set new active room
        m_ActiveRoom = roomToEnter;

        // Enable new room
        EnableRoom(m_ActiveRoom);

        // Cache connecting room spawn point
        Transform spawnTransform = connectingDoor.GetConnectingRoomSpawnPoint(roomToEnter.GetRoomType());

        if (spawnTransform == null)
        {
            Debug.LogError($"Spawn Point Not Found!");
            return;
        }

        // Transport player to spawn pos
        NavMeshAgent playerAgent = m_Player.GetComponent<NavMeshAgent>();
        bool succesfulWarp = playerAgent.Warp(spawnTransform.position);

        // Check for succesfull warp
        if (succesfulWarp) Debug.Log("Agent successfully warped to: " + spawnTransform.position);
        else Debug.LogWarning("Failed to warp agent to: " + spawnTransform.position + ". Is the position on the NavMesh?");

        // Disable The old room
        DisableRoom(roomToLeave);
    }

    private void EnableRoom(Room roomToEnable)
    {
        //Debug.Log($"[Room Manager] Enabling Room: {roomToEnable.name}");
        //if (!roomToEnable.gameObject.activeSelf)
        roomToEnable.gameObject.SetActive(true);
    }

    private void DisableRoom(Room roomToDisable)
    {
        //Debug.Log($"[Room Manager] Disabling Room: {roomToDisable.name}");
        //if (roomToDisable.gameObject.activeSelf)
        roomToDisable.gameObject.SetActive(false);
    }
    

}
