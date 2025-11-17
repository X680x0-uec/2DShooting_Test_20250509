using UnityEngine;
using System.Collections;

public class AlternatingCurveAttack : MonoBehaviour
{
    [SerializeField] private GameObject curvingBulletPrefab;
    [SerializeField] private float fireInterval = 1.0f;
    [SerializeField] private float bulletSpeed = 5f;

    [SerializeField] private int wayCount = 32;

    private Coroutine attackRoutine;
    
    private int curveToggle = 1;

    private void Awake()
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
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            float randomOffsetAngle = Random.Range(0f, 360f);
            
            FireAllDirectionBullets(transform.position, randomOffsetAngle, curveToggle);

            curveToggle *= -1;

            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireAllDirectionBullets(Vector3 spawnPosition, float offsetAngle, int directionToCurve)
    {
        if (curvingBulletPrefab == null) return;
        
        float angleStep = 360f / wayCount;

        for (int i = 0; i < wayCount; i++)
        {
            float currentAngle = offsetAngle + (angleStep * i);
            Vector2 direction = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            );

            GameObject bullet = Instantiate(curvingBulletPrefab, spawnPosition, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;
            }

            CurvingBullet curveScript = bullet.GetComponent<CurvingBullet>();
            if (curveScript != null)
            {
                curveScript.Initialize(directionToCurve);
            }
        }
    }
}