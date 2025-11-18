using UnityEngine;

public class MovingFanShotEnemy : MonoBehaviour
{
    [Header("弾の設定")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float fireInterval = 1f;       // 発射間隔（秒）
    public float bulletLifetime = 5f;

    [Header("上下移動設定")]
    public float moveAmplitude = 2f;      // 振幅
    public float moveSpeed = 2f;          // 上下移動の速度

    private float fireTimer = 0f;
    private Vector3 startPos;

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
            FireLeft();
            fireTimer = 0f;
        }
    }

    void FireLeft()
    {
        // ★全部真左に飛ばす（180度）
        float angle = 180f;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        EnemyBulletContoroller bc = bullet.GetComponent<EnemyBulletContoroller>();
        if (bc != null)
        {
            bc.angle = angle;          // ← 真左
            bc.speed = bulletSpeed;
        }

        Destroy(bullet, bulletLifetime);
    }
}
