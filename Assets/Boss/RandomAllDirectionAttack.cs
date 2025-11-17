using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomAllDirectionAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireInterval = 0.5f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int wayCount = 32;
    [SerializeField] private List<Vector2> fireOffsets;

    private Coroutine attackRoutine;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
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
            foreach (Vector2 offset in fireOffsets)
            {
                float randomOffsetAngle = Random.Range(0f, 360f);
                Vector3 spawnPos = transform.position + (Vector3)offset;

                FireBullet(spawnPos, randomOffsetAngle);
            }

            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireBullet(Vector3 spawnPosition, float offsetAngle)
    {
        if (bulletPrefab == null) return;
        
        float angleStep = 360f / wayCount;

        for (int i = 0; i < wayCount; i++)
        {
            float currentAngle = offsetAngle + (angleStep * i);

            Vector2 direction = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            );

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;
            }
        }
    }
}