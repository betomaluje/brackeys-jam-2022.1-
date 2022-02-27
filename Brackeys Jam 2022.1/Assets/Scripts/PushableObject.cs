using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Push(float force, Vector2 direction)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
