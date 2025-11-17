using UnityEngine;
using System.Collections;

public class PlayerSniperAttack : MonoBehaviour
{
    [Header("攻撃設定")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireInterval = 1.0f;
    [SerializeField] private float bulletSpeed = 4f;
    [SerializeField] private Vector2 fireOffset;

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
        else
        {
            Debug.LogWarning("[PlayerSniperAttack] プレイヤーが見つかりません。");
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

        Vector2 direction = (Vector2)playerTransform.position - (Vector2)spawnPos;
        direction.Normalize();

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}