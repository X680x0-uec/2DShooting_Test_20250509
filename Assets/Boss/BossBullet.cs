using UnityEngine;

public class BossBullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    public float speed = 5f;
    protected Vector2 direction = Vector2.left; // ← 左方向をデフォルトに変更

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        rb.linearVelocity = direction * speed;
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        if (rb != null)
            rb.linearVelocity = direction * speed;
    }

    protected virtual void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
