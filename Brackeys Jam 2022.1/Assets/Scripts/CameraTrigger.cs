using UnityEngine;

namespace DefaultNamespace
{
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private int cameraNumber = 0;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                CameraFollow.OnCameraTriggerReached(cameraNumber, transform);
            }
        }
    }
}