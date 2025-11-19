using UnityEngine;
using System.Collections;

public class RotatingSpiralAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireInterval = 0.05f;
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private int wayCount = 4;

    private Coroutine attackRoutine;
    private float currentAngle = 0f;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        if (attackRoutine != null) StopCoroutine(attackRoutine);
        attackRoutine = StartCoroutine(Attack());
    }

    private void OnDisable()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            float angleStep = 360f / wayCount;
            for (int i = 0; i < wayCount; i++)
            {
                FireBullet(currentAngle + (angleStep * i));
            }
            currentAngle += rotationSpeed;
            
            if (currentAngle >= 360f) currentAngle -= 360f;
            else if (currentAngle < 0f) currentAngle += 360f;

            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireBullet(float angle)
    {
        if (bulletPrefab == null) return;

        Vector2 direction = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}