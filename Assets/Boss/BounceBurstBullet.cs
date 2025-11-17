using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BounceBurstBullet : MonoBehaviour
{
    [SerializeField] private GameObject shrapnelPrefab;
    [SerializeField] private int shrapnelCount = 16;

    [SerializeField] private float minBurstSpeed = 2f;
    [SerializeField] private float maxBurstSpeed = 5f;

    [SerializeField] private float uiAreaHeight = 2f;
    private Vector2 screenBounds;
    private bool hasBurst = false;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Update()
    {
        if (hasBurst) return;

        Vector2 currentPosition = transform.position;

        if (Mathf.Abs(currentPosition.x) > screenBounds.x || currentPosition.y > screenBounds.y || currentPosition.y < -screenBounds.y + uiAreaHeight)
        {
            Burst();
        }
    }

    private void Burst()
    {
        if (hasBurst) return;
        hasBurst = true;

        if (shrapnelPrefab == null)
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < shrapnelCount; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector2 direction = new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            );

            float randomSpeed = Random.Range(minBurstSpeed, maxBurstSpeed);

            GameObject shrapnel = Instantiate(shrapnelPrefab, transform.position, Quaternion.identity);
            
            Rigidbody2D rb = shrapnel.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * randomSpeed;
            }
        }

        Destroy(gameObject);
    }
}