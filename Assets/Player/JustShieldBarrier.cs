using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class JustShieldBarrier : MonoBehaviour
{
    public GameObject debrisSpawnerPrefab;
    public float maxSizeDuration = 1.5f;
    public float shrinkDuration = 0.5f;

    private Vector3 originalScale;
    private CircleCollider2D circleCollider;
    private float originalColliderRadius;

    private float elapsedTime = 0f;
    private float totalDuration;

    void Start()
    {
        originalScale = transform.localScale;
        circleCollider = GetComponent<CircleCollider2D>();
        originalColliderRadius = circleCollider.radius;

        totalDuration = maxSizeDuration + shrinkDuration;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime <= maxSizeDuration)
        {
            transform.localScale = originalScale;
            circleCollider.radius = originalColliderRadius;
        }
        else if (elapsedTime <= totalDuration)
        {
            float shrinkElapsedTime = elapsedTime - maxSizeDuration;
            float progress = shrinkElapsedTime / shrinkDuration;

            float newScaleValue = Mathf.Lerp(originalScale.x, 0f, progress);
            float newRadius = Mathf.Lerp(originalColliderRadius, 0f, progress);

            transform.localScale = new Vector3(newScaleValue, newScaleValue, originalScale.z);
            circleCollider.radius = newRadius;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Instantiate(debrisSpawnerPrefab, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    public void ChangeMaxSizeDuration(float duration)
    {
        maxSizeDuration = duration;
    }
}
