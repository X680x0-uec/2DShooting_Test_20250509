using UnityEngine;
using System.Collections;

public class FourWayOffsetAttack : MonoBehaviour
{
    [Header("攻撃設定")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireInterval = 0.3f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private Vector2 fireOffset;

    private Coroutine attackRoutine;
    private Transform playerTransform;

    private void Awake() { enabled = false; }
    private void OnEnable()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) playerTransform = player.transform;
        attackRoutine = StartCoroutine(Attack());
    }
    private void OnDisable()
    {
        if (attackRoutine != null) { StopCoroutine(attackRoutine); attackRoutine = null; }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (playerTransform != null)
            {
                FireFourWayOffset();
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireFourWayOffset()
    {
        if (bulletPrefab == null) return;
        
        Vector3 spawnPos = transform.position + (Vector3)fireOffset;

        Vector2 directionToPlayer = (Vector2)playerTransform.position - (Vector2)spawnPos;
        
        float centerAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        
        float[] angles = new float[] { -25f, 25f, -10f, 10f }; 

        for (int i = 0; i < 4; i++)
        {
            float finalAngle = centerAngle + angles[i]; 

            Vector2 finalDirection = new Vector2(
                Mathf.Cos(finalAngle * Mathf.Deg2Rad),
                Mathf.Sin(finalAngle * Mathf.Deg2Rad)
            );

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = finalDirection * bulletSpeed;
            }
        }
    }
}