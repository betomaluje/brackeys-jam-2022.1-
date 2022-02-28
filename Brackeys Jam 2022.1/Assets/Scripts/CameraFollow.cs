using System;
using Cinemachine;
using DefaultNamespace;
using TarodevController;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform[] gridPoints;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Debug")] 
    [SerializeField] private bool debug = true;
    [SerializeField, Range(0f, 1f)] private float debugRadius = .5f;

    private static event Action<int, Vector2> CameraTriggerReached;
    public static void OnCameraTriggerReached(int cameraIndex, Vector2 outputPosition) => 
        CameraTriggerReached?.Invoke(cameraIndex, outputPosition);

    private Transform _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void OnEnable()
    {
        CameraTriggerReached += HandleCameraTriggerReached;
    }

    private void OnDisable()
    {
        CameraTriggerReached -= HandleCameraTriggerReached;
    }

    private void HandleCameraTriggerReached(int newCamera, Vector2 outputPosition)
    {
        var targetCamera = gridPoints[newCamera];
        virtualCamera.Follow = targetCamera;
        virtualCamera.LookAt = targetCamera;

        _player.position = outputPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (gridPoints.Length <= 0 || !debug) return;

        for (int i = 0; i < gridPoints.Length; i++)
        {
            var position = gridPoints[i].position;
            Gizmos.DrawWireSphere(position, debugRadius);
            Handles.Label(position, $"Point {i}");
        }
    }
}
