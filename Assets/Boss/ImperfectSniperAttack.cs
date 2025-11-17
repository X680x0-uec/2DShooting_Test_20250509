using UnityEngine;
using System.Collections;

public class ImperfectSniperAttack : MonoBehaviour
{
    [Header("攻撃設定")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireInterval = 0.1f;
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private Vector2 fireOffset;

    [Header("ブレ設定")]
    [SerializeField, Tooltip("弾がブレる最大の角度（例: 15度なら -15〜+15 の間でブレる）")]
    private float spreadAngle = 10f; 

    private Coroutine attackRoutine;
    private Transform playerTransform;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            playerTransform = player.transform;
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
            if (playerTransform != null)
            {
                FireBullet();
            }
            
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab == null) return;

        Vector3 spawnPos = transform.position + (Vector3)fireOffset;

        Vector2 directionToPlayer = (Vector2)playerTransform.position - (Vector2)spawnPos;

        float randomAngleOffset = Random.Range(-spreadAngle, spreadAngle);

        Vector2 finalDirection = Quaternion.Euler(0, 0, randomAngleOffset) * directionToPlayer;
        finalDirection.Normalize();

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = finalDirection * bulletSpeed;
        }
    }
}