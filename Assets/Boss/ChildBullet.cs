using UnityEngine;

public class ChildBullet : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 3f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // rb.linearVelocity は ParentBullet 側で設定されるので、ここでは上書きしない
        if (rb != null && rb.linearVelocity == Vector2.zero)
        {
            rb.linearVelocity = Vector2.left * speed; // 念のため、親から速度が渡らなかった時の保険
        }

        Destroy(gameObject, lifetime);
    }
}
