using System;
using Cinemachine;
using TarodevController;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform[] gridPoints;
    [SerializeField] private float xPosOffset = 2f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private static event Action<int, Transform> CameraTriggerReached;
    public static void OnCameraTriggerReached(int cameraIndex, Transform trigger) => 
        CameraTriggerReached?.Invoke(cameraIndex, trigger);

    private int currentCamera;
    private Transform player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void OnEnable()
    {
        CameraTriggerReached += HandleCameraTriggerReached;
    }

    private void OnDisable()
    {
        CameraTriggerReached -= HandleCameraTriggerReached;
    }

    private void HandleCameraTriggerReached(int newCamera, Transform trigger)
    {
        var camera = gridPoints[newCamera];
        virtualCamera.Follow = camera;
        virtualCamera.LookAt = camera;

        var xOffset = currentCamera < newCamera ? xPosOffset : -xPosOffset;
        var playerPos = trigger.position;
        playerPos.x += xOffset;
        player.position = playerPos;
        
        currentCamera = newCamera;
    }
    
}
