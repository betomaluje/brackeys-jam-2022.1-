using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraTrigger : MonoBehaviour
    {
        [Header("Player Placement")]
        [SerializeField] private int cameraAtLeft = NO_CAMERA;
        [SerializeField] private int cameraAtRight = NO_CAMERA;
        [SerializeField] private float nextPlayerXOffset = 2f;
        [Header("Player Detection")]
        [SerializeField] private LayerMask targetMask;
        [SerializeField, Range (0f, 1f)] private float rayOffset = .25f;
        [SerializeField] private float rayLength = 1f;
        [SerializeField, Range(0, 6)] private int amountOfRays = 3;

        private const int NO_CAMERA = -1;

        private void Update()
        {
            for (int i = 0; i < amountOfRays; i++)
            {
                // we go from 1 to -1
                Vector2[] rayPositions = CalculateRays(i);
                RaycastHit2D leftHit = Physics2D.Linecast(transform.position, rayPositions[0], targetMask);
                if (leftHit && cameraAtRight != NO_CAMERA)
                {
                    // we need to go right
                    CameraFollow.OnCameraTriggerReached(cameraAtRight, 
                        CalculateNextPlayerPosition(CastDirection.Right));
                }
                
                RaycastHit2D rightHit = Physics2D.Linecast(transform.position, rayPositions[1], targetMask);
                if (rightHit && cameraAtLeft != NO_CAMERA)
                {
                    // we need to go left
                    CameraFollow.OnCameraTriggerReached(cameraAtLeft, 
                        CalculateNextPlayerPosition(CastDirection.Left));
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

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < amountOfRays; i++)
            {
                Gizmos.color = Color.magenta;
                Vector2[] rayPositions = CalculateRays(i);

                Gizmos.DrawLine(transform.position, rayPositions[0]);
                Handles.Label(rayPositions[0], $"L {i} y: {rayPositions[0].y}");
                Gizmos.DrawLine(transform.position, rayPositions[1]);
                Handles.Label(rayPositions[1], $"R {i} y: {rayPositions[1].y}");
            }

            Gizmos.color = Color.blue;
            var playerPosLeft = transform.position;
            
            if (cameraAtLeft != NO_CAMERA)
            {
                playerPosLeft.x -= nextPlayerXOffset;
                Gizmos.DrawWireSphere(playerPosLeft, 1);
            }

            Gizmos.color = Color.red;
            var playerPosRight = transform.position;
            if (cameraAtRight != NO_CAMERA)
            {
                playerPosRight.x += nextPlayerXOffset;
                Gizmos.DrawWireSphere(playerPosRight, 1);
            }
        }
    }
}