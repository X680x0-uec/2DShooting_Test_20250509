using UnityEngine;
using System.Collections;

public class SineWaveAttack : MonoBehaviour
{
    [Header("弾の設定")]
    public GameObject bulletPrefab; // 弾のプレハブ
    public Transform firePoint;     // 発射位置
    public float fireInterval = 0.2f; // 発射間隔
    public float amplitude = 1f;    // 振幅（上下の幅）
    public float frequency = 2f;    // 周波数（波の速さ）
    public float bulletSpeed = 5f;  // 弾の進む速さ
    public float fireRangeY = 2f;   // 発射位置のY座標のランダム範囲

    private bool isAttacking = false;

    void Start()
    {
        enabled = false; // AttackCycleに制御させるので最初は無効
    }

    void OnEnable()
    {
        if (!isAttacking)
            StartCoroutine(Attack());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        isAttacking = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        while (enabled)
        {
            if (bulletPrefab != null && firePoint != null)
            {
                // 発射位置をランダムに上下させる
                Vector3 spawnPos = firePoint.position;
                spawnPos.y += Random.Range(-fireRangeY, fireRangeY);

                // 弾を生成
                GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

                // 弾に波の情報を渡す（位相を -π〜π の範囲でランダムに）
                float phase = Random.Range(-Mathf.PI, Mathf.PI);

                SineWaveBullet sine = bullet.GetComponent<SineWaveBullet>();
                if (sine != null)
                {
                    sine.SetWave(Vector2.left, bulletSpeed, amplitude, frequency, phase);
                }
            }

            yield return new WaitForSeconds(fireInterval);
        }

        isAttacking = false;
    }
}
