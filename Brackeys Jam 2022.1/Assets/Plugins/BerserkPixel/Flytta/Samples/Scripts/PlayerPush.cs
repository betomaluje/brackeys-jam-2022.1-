using TarodevController;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float range = 1f;
    public LayerMask targetLayer;
    public Transform pushOrigin;
    public float force;

    private Vector2 _difference;
    private PlayerInput _playerInput;

    private void Start()
    {
        _playerInput = GetComponent<IPlayerController>().PlayerInput;
        _playerInput.OnPushPressed += HandlePushPressed;
        _playerInput.OnMovementXChange += HandleMovementXChange;
    }

    private void OnDisable()
    {
        _playerInput.OnPushPressed -= HandlePushPressed;
        _playerInput.OnMovementXChange -= HandleMovementXChange;
    }

    private void HandlePushPressed()
    {
        Push();
    }

    private void HandleMovementXChange(float x)
    {
        if (x != 0) pushOrigin.localScale = new Vector3(x > 0 ? 1 : -1, 1, 1);
    }

    private void Push()
    {
        RaycastHit2D hit = Physics2D.Linecast(pushOrigin.position,
            pushOrigin.position + pushOrigin.localScale.x * Vector3.right * range,
            targetLayer);
        if (hit && hit.transform.TryGetComponent(out PushableObject pushableObject))
        {
            _difference = -((Vector2)pushOrigin.position - hit.point).normalized;
            pushableObject.Push(force, _difference);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(pushOrigin.position, pushOrigin.position + pushOrigin.localScale.x * Vector3.right * range);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pushOrigin.right, _difference * force);
    }
}