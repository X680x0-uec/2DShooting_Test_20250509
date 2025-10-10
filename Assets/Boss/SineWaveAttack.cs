using UnityEngine;
using System.Collections;

public class SineWaveAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 0.4f;
    [SerializeField] private float fireYRange = 1.0f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float frequency = 3f;
    [SerializeField] private float amplitude = 0.5f;

    private Coroutine attackRoutine;

    private void Start()
    {
        enabled = false;
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
            spawnPos.y += Random.Range(-fireYRange, fireYRange);

            GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            SineWaveBullet bullet = bulletObj.GetComponent<SineWaveBullet>();
            bullet.SetWave(Vector2.left, speed, frequency, amplitude);

            yield return new WaitForSeconds(fireInterval);
        }
    }
}
