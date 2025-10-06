using UnityEngine;
using EC;

[CreateAssetMenu(fileName = "NewDoorData", menuName = "EC/Room/DoorData")]
public class DoorInteractableData : BaseInteractableData
{
    [Header("Door Interactable Configuration")]
    [Tooltip("One of the two rooms this door connects. [Assign in Editor].")]
    public RoomType roomA;

    [Tooltip("One of the two rooms this door connects. [Assign in Editor]")]
    public RoomType roomB;

}
