using UnityEngine;

public class BossHPAutoDamage : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 5f;   // 1回ごとのダメージ量
    [SerializeField] private float interval = 1.0f;      // ダメージを与える間隔(秒)
    private BossHP bossHP;

    private void Start()
    {
        bossHP = GetComponent<BossHP>();
        if (bossHP == null)
        {
            Debug.LogError("[BossHPAutoDamage] BossHPが見つかりません！");
            enabled = false;
            return;
        }

        InvokeRepeating(nameof(DealDamage), interval, interval);
    }

    private void DealDamage()
    {
        if (bossHP != null)
        {
            bossHP.TakeDamage(damagePerTick);
            Debug.Log($"[BossHPAutoDamage] {damagePerTick}ダメージを与えました");
        }
    }
}
