using UnityEngine;

public class EnemyHoming : MonoBehaviour
{
    [Header("弾")]
    public GameObject bulletPrefab;   // 弾のプレハブ
    public float bulletSpeed = 5f;

    [Header("移動")]
    public float moveSpeed = 2f;      // 左への移動速度

    [Header("自機狙い弾")]
    public float aimedInterval = 1f;  // 弾を撃つ間隔
    private float aimedTimer = 0f;

    void Update()
    {
        // --------------------------
        // 左に移動（ワールド座標で強制）
        transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);

        // --------------------------
        // 自機狙い弾
        aimedTimer += Time.deltaTime;
        if(aimedTimer >= aimedInterval)
        {
            aimedTimer = 0f;
            ShootAimed();
        }
    }

    // --------------------------
    void ShootAimed()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null) return;

        // プレイヤーの方向ベクトル
        Vector2 dir = (player.transform.position - transform.position).normalized;

        // 弾の角度を計算
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 弾を生成
        SpawnBullet(angle);
    }

    void SpawnBullet(float angle)
    {
        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        EnemyBulletContoroller enemybullet = b.GetComponent<EnemyBulletContoroller>();
        enemybullet.angle = angle;
        enemybullet.speed = bulletSpeed;
    }
}
