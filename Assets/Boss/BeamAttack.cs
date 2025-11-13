using UnityEngine;
using System.Collections;

public class BeamAttack : MonoBehaviour
{
    [Header("攻撃設定")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float fireYRange = 1.0f; // 発射位置のY座標をランダム化

    private Coroutine attackRoutine;

    private void Awake()
    {
        // StartではなくAwakeで制御（Startでenabled=falseにするとOnEnableが呼ばれない場合がある）
        enabled = false;
    }

    private void OnEnable()
    {
        Debug.Log("[BeamAttack] Enabled!");

        // もし以前のCoroutineが残っていれば安全に停止してから再開
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }

        attackRoutine = StartCoroutine(Attack());
    }

    private void OnDisable()
    {
        Debug.Log("[BeamAttack] Disabled!");

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
            if (bulletPrefab != null && firePoint != null)
            {
                Vector3 spawnPos = firePoint.position;
                spawnPos.y += Random.Range(-fireYRange, fireYRange); // Yをランダム化

                GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.left * bulletSpeed; // ← 左方向
                }
            }
            else
            {
                Debug.LogWarning("[BeamAttack] bulletPrefab または firePoint が設定されていません。");
            }

            yield return new WaitForSeconds(fireInterval);
        }
    }
}
