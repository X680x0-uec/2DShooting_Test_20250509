using UnityEngine;

public class MovingFanShotEnemy : MonoBehaviour
{
    [Header("弾の設定")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public int bulletCount = 5;           // 扇型の弾数
    public float fireInterval = 1f;       // 発射間隔（秒）
    public float bulletLifetime = 5f;

    [Header("扇型設定")]
    public float spreadAngle = 60f;       // 扇の角度（度）

    [Header("上下移動設定")]
    public float moveAmplitude = 2f;      // 上下の振幅
    public float moveSpeed = 2f;          // 上下移動の速度

    private float fireTimer = 0f;
    private Vector3 startPos;             // 移動の基準位置

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // --- 敵の上下移動 ---
        float yOffset = Mathf.Sin(Time.time * moveSpeed) * moveAmplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);

        // --- 弾の発射 ---
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            FireFan();
            fireTimer = 0f;
        }
    }

    void FireFan()
    {
        float startAngle = -spreadAngle / 2f;
        float angleStep = bulletCount > 1 ? spreadAngle / (bulletCount - 1) : 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;

        // 左向きを基準に角度を調整
            float rad = Mathf.Deg2Rad * angle; 
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)); // 基準は右方向
            dir = Quaternion.Euler(0, 0, 180f) * dir; // 左方向に回転

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = dir.normalized * bulletSpeed;
            }

            Destroy(bullet, bulletLifetime);
        }
    }


}
