using System.Collections;
using UnityEngine;

public class SplitAttack : MonoBehaviour
{
    [Header("攻撃設定")]
    [SerializeField] private GameObject parentBulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 2f;

    private Coroutine attackRoutine;

    private void Awake()
    {
        // Awakeで無効化（StartでやるとOnEnableが動かないケースがある）
        enabled = false;
    }

    private void OnEnable()
    {
        Debug.Log("[SplitAttack] Enabled!");

        // 以前のコルーチンが残っている可能性を排除
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }

        attackRoutine = StartCoroutine(FireRoutine());
    }

    private void OnDisable()
    {
        Debug.Log("[SplitAttack] Disabled!");

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            if (parentBulletPrefab != null && firePoint != null)
            {
                Instantiate(parentBulletPrefab, firePoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("[SplitAttack] parentBulletPrefab または firePoint が設定されていません。");
            }

            yield return new WaitForSeconds(fireInterval);
        }
    }
}

