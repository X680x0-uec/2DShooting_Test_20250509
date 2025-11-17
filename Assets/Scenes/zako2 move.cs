using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 1.0f;
    public float bulletAngle = 180f; // 弾は左へ
    public float moveSpeed = 2f;     // 敵が左に動くスピード

    void Start()
    {
        InvokeRepeating(nameof(Shoot), 0f, shootInterval);
    }

    void Update()
    {
        // 敵を左へ移動
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    void Shoot()
    {
        // 敵の位置から弾を生成
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // 弾の角度を設定
        EnemyBulletContoroller bulletCtrl = bullet.GetComponent<EnemyBulletContoroller>();
        bulletCtrl.angle = bulletAngle;
    }
}
