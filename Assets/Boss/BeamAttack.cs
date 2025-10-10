using UnityEngine;
using System.Collections;

public class BeamAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float fireYRange = 1.0f; // 発射位置のY座標をランダム化

    private Coroutine attackRoutine;

    private void Start()
    {
        enabled = false; // 起動時は無効
    }

    private void OnEnable()
    {
        attackRoutine = StartCoroutine(Attack());
    }

    private void OnDisable()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            Vector3 spawnPos = firePoint.position;
            spawnPos.y += Random.Range(-fireYRange, fireYRange); // Yをランダム化

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.left * bulletSpeed; // ← 左方向

            yield return new WaitForSeconds(fireInterval);
        }
    }
}
