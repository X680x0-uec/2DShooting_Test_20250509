using UnityEngine;
using System.Collections;

public class SineWaveAttack : MonoBehaviour
{
    [Header("弾の設定")]
    [SerializeField] private GameObject bulletPrefab; // 弾のプレハブ
    [SerializeField] private Transform firePoint;     // 発射位置
    [SerializeField] private float fireInterval = 0.2f; // 発射間隔
    [SerializeField] private float amplitude = 1f;    // 振幅（上下の幅）
    [SerializeField] private float frequency = 2f;    // 周波数（波の速さ）
    [SerializeField] private float bulletSpeed = 5f;  // 弾の進む速さ
    [SerializeField] private float fireRangeY = 2f;   // 発射位置のY座標のランダム範囲

    private Coroutine attackRoutine;

    private void Awake()
    {
        // Startでenabled=falseにしないよう変更
        // WaveManagerが制御する
    }

    private void OnEnable()
    {
        Debug.Log("[SineWaveAttack] Enabled!");

        // firePoint または bulletPrefab が設定されていないと動作しない
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("[SineWaveAttack] bulletPrefab または firePoint が未設定です！");
            return;
        }

        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(Attack());
        }
    }

    private void OnDisable()
    {
        Debug.Log("[SineWaveAttack] Disabled!");
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
            // 発射位置をランダムに上下させる
            Vector3 spawnPos = firePoint.position;
            spawnPos.y += Random.Range(-fireRangeY, fireRangeY);

            // 弾を生成
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

            // 弾に波の情報を渡す
            float phase = Random.Range(-Mathf.PI, Mathf.PI);
            var sine = bullet.GetComponent<SineWaveBullet>();
            if (sine != null)
            {
                sine.SetWave(Vector2.left, bulletSpeed, amplitude, frequency, phase);
            }

            yield return new WaitForSeconds(fireInterval);
        }
    }
}
