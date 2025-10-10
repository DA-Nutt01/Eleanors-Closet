using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(Collider))]
public class RoomArea : MonoBehaviour
{
    /*
    Single Responsibility:
    This script associates a cinemachine camera with a given area of a room & activates that camera
    when the area is entered.
    This script is to be attached to empt game objects with a trigger attached to it.
    */
    [Tooltip("A ref to the associated cinemachine camera of this room area.")]
    [SerializeField] private CinemachineCamera m_AssociatedCamera;

    private void OnTriggerEnter(Collider collider)
    {
        // Check if Collider is the player's
        if (collider.CompareTag("Player"))
        {
            Debug.Log($"Entering Room Area: {this.gameObject.name}");
            // Tell CameraManager to switch this area's camera on
            CameraManager.Instance.SwitchToCamera(m_AssociatedCamera);
        }
    }
}
