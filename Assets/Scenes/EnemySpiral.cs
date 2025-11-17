using UnityEngine;

public class EnemySpiral : MonoBehaviour
{
    [Header("弾")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;

    [Header("移動")]
    public float moveSpeed = 2f;

    [Header("スパイラル")]
    public float spiralInterval = 0.1f;   // 弾を撃つ間隔
    private float spiralTimer = 0f;
    private float currentAngle = 0f;      // 弾の角度

    public float angleStep = 20f;         // 弾を回転させる角度

    void Update()
    {
        // 左に移動
        transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);

        // スパイラル弾
        spiralTimer += Time.deltaTime;
        if(spiralTimer >= spiralInterval)
        {
            spiralTimer = 0f;
            ShootSpiral();
        }
    }

    void ShootSpiral()
    {
        // 弾を生成
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        EnemyBulletContoroller bullet = b.GetComponent<EnemyBulletContoroller>();

        // 左方向にベース角度をセットして、らせんを作るためにcurrentAngleを加算
        float angle = 180f + currentAngle; // 180度で左方向
        bullet.angle = angle;
        bullet.speed = bulletSpeed;

        // 次の弾の角度を少しずつ増やす
        currentAngle += angleStep;
        if (currentAngle >= 360f) currentAngle -= 360f;
    }
}
