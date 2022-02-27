using TarodevController;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float Range = 2f;
    public LayerMask targetLayer;
    public Transform pushOrigin;
    public float force;

    private Vector2 difference;
    private IPlayerController _player;
    
    void Awake() => _player = GetComponent<IPlayerController>();
    
    private void Update()
    {
        if (_player.Input.X != 0) pushOrigin.localScale = new Vector3(_player.Input.X > 0 ? 1 : -1, 1, 1);
        
        if (_player.Input.Push)
        {
            Push();
        }
    }

    private void Push()
    {
        RaycastHit2D hit = Physics2D.Linecast(pushOrigin.position, 
            pushOrigin.position + pushOrigin.localScale.x * Vector3.right * Range, 
            targetLayer);
        if (hit && hit.transform.TryGetComponent(out PushableObject pushableObject))
        { 
            difference = -((Vector2)pushOrigin.position - hit.point).normalized;
            pushableObject.Push(force, difference);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(pushOrigin.position, pushOrigin.position + pushOrigin.localScale.x * Vector3.right * Range);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pushOrigin.right, difference * force); 
    }
}
