using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    /*
    Single Responsibility:
    This script manages the priority of cinemachine cameras in a given room & toggles them on based
    on what RoomArea the player is in
    */

    public static CameraManager Instance { get; private set; }
    [SerializeField] private CinemachineCamera m_ActiveCamera;

    private void Awake()
    {
        // Check Singleton
        if (Instance == null & Instance != this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Cannot have more than one instance of CameraManger!");
            Destroy(gameObject);
            return;
        }
    } 

    private void Start()
    {
        if (m_ActiveCamera == null)
        {
            Debug.Log($"ActiveCamera is empty.");
            return;
        }
        
        SwitchToCamera(m_ActiveCamera);
    }

    public void SwitchToCamera(CinemachineCamera newCamera)
    {
        // Check if the same area is trying to be entered
        if (m_ActiveCamera == newCamera) return;
        //Check if there is an active camera & Deactivate it
        if (m_ActiveCamera != null) m_ActiveCamera.Priority = 0;
        // Activate new camera
        m_ActiveCamera = newCamera;
        m_ActiveCamera.Priority = 10;

    }
}
