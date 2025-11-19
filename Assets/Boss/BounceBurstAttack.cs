using UnityEngine;
using System.Collections;

public class BounceBurstAttack : MonoBehaviour
{
    [SerializeField] private GameObject bounceBulletPrefab;
    [SerializeField] private float fireInterval = 0.7f;
    [SerializeField] private float bulletSpeed = 7f;

    private Coroutine attackRoutine;

    // 飛ばす方向のベクトル (正規化済み)
    private Vector2[] directions = new Vector2[]
    {
        new Vector2(-4, 1).normalized,
        new Vector2(-4, -1).normalized,
        new Vector2(-2, 1).normalized,
        new Vector2(-2, -1).normalized,
    };

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
            foreach (Vector2 dir in directions)
            {
                FireBullet(dir);
            }
            
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void FireBullet(Vector2 direction)
    {
        if (bounceBulletPrefab == null) return;

        GameObject bullet = Instantiate(bounceBulletPrefab, transform.position, Quaternion.identity);
        
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}