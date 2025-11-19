using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BounceBurstBullet : MonoBehaviour
{
    [SerializeField] private GameObject shrapnelPrefab;
    [SerializeField] private int wayCount = 16;

    [SerializeField] private float bulletSpeed = 2f;

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

        float randomAngle = Random.Range(0f, 360f);
        float angleStep = 360f / wayCount;

        for (int i = 0; i < wayCount; i++)
        {
            float currentAngle = randomAngle + (angleStep * i);
            Vector2 direction = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            );

            GameObject shrapnel = Instantiate(shrapnelPrefab, transform.position, Quaternion.identity);
            
            Rigidbody2D rb = shrapnel.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;
            }
        }

        Destroy(gameObject);
    }
}