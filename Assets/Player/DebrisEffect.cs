using UnityEngine;

public class DebrisEffect : MonoBehaviour
{
    public float lifeTime = 1.5f;
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected float elapsedTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"{gameObject}: SpriteRenderer が見つかりません。");
            Destroy(gameObject);
            return;
        }

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        rb.linearVelocity = randomDirection * randomSpeed;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        float progress = elapsedTime / lifeTime;

        float alpha = 1.0f - progress;
        
        Color newColor = spriteRenderer.color;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
    }
}
