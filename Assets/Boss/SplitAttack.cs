using System.Collections;
using UnityEngine;

public class SplitAttack : MonoBehaviour
{
    public GameObject parentBulletPrefab;
    public Transform firePoint;
    public float fireInterval = 2f;

    private Coroutine attackRoutine;

    void Start()
    {
        enabled = false; // AttackCycleが制御
    }

    void OnEnable()
    {
        attackRoutine = StartCoroutine(FireRoutine());
    }

    void OnDisable()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            if (parentBulletPrefab != null && firePoint != null)
            {
                Instantiate(parentBulletPrefab, firePoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
