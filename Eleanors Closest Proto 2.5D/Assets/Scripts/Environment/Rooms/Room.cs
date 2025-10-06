using System.Collections.Generic;
using EC;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Configuration"), Space(5)]
    [Tooltip("Scriptable data asset defining doors, room layout, item spawn info, etc.")]
    [SerializeField]
    private RoomData m_RoomData;

    //[Tooltip("Transforms where items can spawn or be placed in this room")]
    //[SerializeField]
    //private List<Transform> m_ItemSpawnLocations = new List<Transform>();

    //[Tooltip("The camera GameObject specific to this room (Cinemachine VCam or similar)")]
    //[SerializeField]
    //private GameObject m_RoomCamera;

    [Tooltip("Identifier for this room (used for lookup, transitions, etc.)")]
    public RoomType m_RoomType;

    private void Awake()
    {
        // Initialize Members
        //m_ItemSpawnLocations = m_RoomData.m_ItemSpawnLocations;
        m_RoomType = m_RoomData.m_RoomType;
    }

    private void Start()
    {
        // Register this room with the room manager
        RoomManager.Instance.RegisterRoom(this);
    }

    public RoomType GetRoomType()
    {
        return m_RoomType;
    }

}
