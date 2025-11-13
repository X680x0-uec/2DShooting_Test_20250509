using UnityEngine;

public class AcceleratingBulletMovement : MonoBehaviour
{
    public float initialSpeed = 3f;
    public float acceleration = 10f;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * initialSpeed;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.right * acceleration, ForceMode2D.Force);
    }
}
