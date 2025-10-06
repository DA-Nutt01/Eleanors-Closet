using UnityEngine;
using System.Collections.Generic;
using EC;

[CreateAssetMenu(fileName = "NewRoomData", menuName = "EC/Room/RoomData")]
public class RoomData : ScriptableObject
{
    public RoomType m_RoomType;
    //[SerializeField] private GameObject m_RoomPrefab;
    public List<DoorInteractable> m_Doors { get; private set; }
    public List<Transform> m_ItemSpawnLocations { get; private set; }
}
