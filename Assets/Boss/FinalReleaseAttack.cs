using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinalReleaseAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireInterval = 0.7f;
    [SerializeField] private int bulletPerShoot = 2;
    [SerializeField] private float minBurstSpeed = 2f;
    [SerializeField] private float maxBurstSpeed = 5f;

    [SerializeField] private List<Vector2> fireOffsets;
    [SerializeField] private Color colorA = Color.red;
    [SerializeField] private Color colorB = Color.green;

    private Coroutine attackRoutine;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
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
                Vector3 spawnPos = transform.position + (Vector3)offset;

                FireBullet(spawnPos);
            }

            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireBullet(Vector3 spawnPosition)
    {
        if (bulletPrefab == null) return;

        for (int i = 0; i < bulletPerShoot; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector2 direction = new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            );

            float randomSpeed = Random.Range(minBurstSpeed, maxBurstSpeed);

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * randomSpeed;
            }

            SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                int choice = Random.Range(0, 2);
                sr.color = (choice == 0) ? colorA : colorB;
            }
        }
    }
}