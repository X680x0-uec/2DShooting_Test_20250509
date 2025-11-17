using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BombProjectile : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float acceleration = 20f;
    private float damageToDeal = 0f;

    public GameObject explosionPrefab;
    public AudioClip bombExplosionSound;

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

    public void Initialize(float damage)
    {
        damageToDeal = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zako") || other.CompareTag("Boss"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        SoundManager.Instance.PlaySound(bombExplosionSound);
        if (explosionPrefab != null)
        {
            GameObject newExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            AreaDamageExplosion explosionScript = newExplosion.GetComponent<AreaDamageExplosion>();
            if (explosionScript != null)
            {
                explosionScript.SetDamage(damageToDeal);
            }
        }

        Destroy(gameObject);
    }
}
