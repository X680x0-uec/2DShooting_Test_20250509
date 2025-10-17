using UnityEngine;

public class NormalBulletMovement : MonoBehaviour
{
    public float speed = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
    }
}
