using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PointAtVelocity : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity == Vector2.zero)
        {
            return;
        }

        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 180f;
    }
}