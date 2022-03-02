using UnityEditor;
using UnityEngine;

namespace BerserkPixel.Flytta
{
    public class CameraTrigger : MonoBehaviour
    {
        [Header("Player Placement")] [SerializeField]
        private Transform cameraAtLeft;

        [SerializeField] private Transform cameraAtRight;
        [SerializeField] private float nextPlayerXOffset = 2f;

        [Header("Player Detection")] [SerializeField]
        private LayerMask targetMask;

        [SerializeField, Range(0f, 1f)] private float rayOffset = .25f;
        [SerializeField] private float rayLength = 1f;
        [SerializeField, Range(0, 6)] private int amountOfRays = 3;

        [Header("Debug")] [SerializeField] private bool debug = true;
        [SerializeField, Range(0f, 1f)] private float debugRadius = .5f;
        [SerializeField] private bool useLabels = true;

        private void Update()
        {
            for (int i = 0; i < amountOfRays; i++)
            {
                // we go from 1 to -1
                Vector2[] rayPositions = CalculateRays(i);
                RaycastHit2D leftHit = Physics2D.Linecast(transform.position, rayPositions[0], targetMask);
                if (leftHit && cameraAtRight != null)
                {
                    // we need to go right
                    CameraFollow.OnCameraTriggerReached(
                        cameraAtRight.position,
                        CalculateNextPlayerPosition(CastDirection.Right)
                    );
                }

                RaycastHit2D rightHit = Physics2D.Linecast(transform.position, rayPositions[1], targetMask);
                if (rightHit && cameraAtLeft != null)
                {
                    // we need to go left
                    CameraFollow.OnCameraTriggerReached(
                        cameraAtLeft.position,
                        CalculateNextPlayerPosition(CastDirection.Left)
                    );
                }
            }
        }

        private Vector2 CalculateNextPlayerPosition(CastDirection direction)
        {
            var xOffset = nextPlayerXOffset * (int)direction;
            var playerPos = transform.position;
            playerPos.x += xOffset;

            return playerPos;
        }

        private Vector2[] CalculateRays(int index)
        {
            Vector2[] rayPositions = new Vector2[2];

            var sign = (index % 2 == 0) ? 1 : -1;

            Vector2 rightRay = transform.position;
            Vector2 leftRay = rightRay;

            var yOffset = rayOffset * sign * (1 + index);
            leftRay.y += yOffset;
            rightRay.y += yOffset;

            leftRay.x += rayLength * -1;
            rightRay.x += rayLength * 1;

            rayPositions[0] = leftRay;
            rayPositions[1] = rightRay;

            return rayPositions;
        }

        #region Debug

        private void OnDrawGizmosSelected()
        {
            if (amountOfRays <= 0 || !debug) return;

            for (int i = 0; i < amountOfRays; i++)
            {
                Gizmos.color = Color.white;
                Vector2[] rayPositions = CalculateRays(i);

                var position = transform.position;
                Gizmos.DrawLine(position, rayPositions[0]);
                Gizmos.DrawLine(position, rayPositions[1]);

                if (useLabels)
                {
                    Handles.Label(rayPositions[0], $"L {i} y: {rayPositions[0].y}");
                    Handles.Label(rayPositions[1], $"R {i} y: {rayPositions[1].y}");
                }
            }

            Gizmos.color = Color.blue;
            if (cameraAtLeft != null)
            {
                Gizmos.DrawWireSphere(CalculateNextPlayerPosition(CastDirection.Left), debugRadius);
                Gizmos.DrawLine(transform.position, cameraAtLeft.position);
            }

            Gizmos.color = Color.red;
            if (cameraAtRight != null)
            {
                Gizmos.DrawWireSphere(CalculateNextPlayerPosition(CastDirection.Right), debugRadius);
                Gizmos.DrawLine(transform.position, cameraAtRight.position);
            }
        }

        #endregion
    }
}