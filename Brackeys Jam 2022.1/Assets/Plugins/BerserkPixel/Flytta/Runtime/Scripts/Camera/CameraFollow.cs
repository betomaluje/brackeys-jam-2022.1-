using System;
using UnityEngine;

namespace BerserkPixel.Flytta
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform player;
        [SerializeField] private float lerpSpeed = 5f;
        
        private static event Action<Vector2, Vector2> CameraTriggerReached;

        public static void OnCameraTriggerReached(Vector2 targetFollowPosition, Vector2 playerOutputPosition) =>
            CameraTriggerReached?.Invoke(targetFollowPosition, playerOutputPosition);

        private Vector3 targetPosition;

        private void Awake()
        {
            targetPosition = mainCamera.transform.position;
        }

        private void OnEnable()
        {
            CameraTriggerReached += HandleCameraTriggerReached;
        }

        private void OnDisable()
        {
            CameraTriggerReached -= HandleCameraTriggerReached;
        }

        private void LateUpdate()
        {
            var cameraPosition = Vector3.Lerp(
                mainCamera.transform.position,
                targetPosition,
                lerpSpeed * Time.deltaTime
            );

            mainCamera.transform.position = cameraPosition;
        }

        private void HandleCameraTriggerReached(Vector2 targetFollowPosition, Vector2 playerOutputPosition)
        {
            Vector3 targetCameraPosition = targetFollowPosition;
            // we do this so we don't lose the z position 
            targetCameraPosition.z = targetPosition.z;
            targetPosition = targetCameraPosition;

            player.position = playerOutputPosition;
        }
    }
}