using UnityEngine;

namespace DefaultNamespace
{
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private int cameraAtLeft = 0;
        [SerializeField] private int cameraAtRight = 1;
        [SerializeField] private float xPosOffset = 2f;
        [SerializeField] private LayerMask targetMask;
        [SerializeField, Range (0f, 1f)] private float rayOffset = .25f;
        [SerializeField] private float rayLength = 1f;
        [SerializeField] private int amountOfCasts = 4;
        
        private const int NO_CAMERA = -1;

        private void Update()
        {
            for (int i = 0; i < amountOfCasts; i++)
            {
                // we go from 1 to -1
                Vector2[] rayPositions = CalculateRays(i);
                
                RaycastHit2D leftHit = Physics2D.Linecast(transform.position, rayPositions[0], targetMask);
                if (leftHit && cameraAtRight != NO_CAMERA)
                {
                    // we need to go right
                    var xOffset = xPosOffset * (int)CastDirection.Right;
                    var playerPos = transform.position;
                    playerPos.x += xOffset;
                    CameraFollow.OnCameraTriggerReached(cameraAtRight, playerPos);
                }
                
                RaycastHit2D rightHit = Physics2D.Linecast(transform.position, rayPositions[1], targetMask);
                if (rightHit && cameraAtLeft != NO_CAMERA)
                {
                    // we need to go left
                    var xOffset = xPosOffset * (int)CastDirection.Left;
                    var playerPos = transform.position;
                    playerPos.x += xOffset;
                    CameraFollow.OnCameraTriggerReached(cameraAtLeft, playerPos);
                }
            }
        }

        private Vector2[] CalculateRays(int index)
        {
            Vector2[] rayPositions = new Vector2[2];

            var sign = (index % 2 == 0) ? 1 : -1;
            
            Vector2 rightRay = transform.position;
            Vector2 leftRay = rightRay;

            var yOffset = rayOffset * index * sign;
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
            for (int i = 0; i < amountOfCasts; i++)
            {
                Vector2[] rayPositions = CalculateRays(i);

                Gizmos.DrawLine(transform.position, rayPositions[0]);
                Gizmos.DrawLine(transform.position, rayPositions[1]);
            }

            Gizmos.color = Color.blue;
            var playerPosLeft = transform.position;
            
            if (cameraAtLeft != NO_CAMERA)
            {
                playerPosLeft.x -= xPosOffset;
                Gizmos.DrawWireSphere(playerPosLeft, 1);
            }

            Gizmos.color = Color.red;
            var playerPosRight = transform.position;
            if (cameraAtRight != NO_CAMERA)
            {
                playerPosRight.x += xPosOffset;
                Gizmos.DrawWireSphere(playerPosRight, 1);
            }
        }
    }
}