using UnityEngine;

public class BossHP : MonoBehaviour
{
    public SkillSystem skillSystem;
    public float maxHP = 100;
    private float currentHP;
    public int pointValue = 100;

    // 無敵状態フラグ
    public bool IsInvincible { get; private set; } = false;

    void Start()
    {
        currentHP = maxHP;
    }

    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;

    // ダメージを受ける関数
    public void TakeDamage(float damage)
    {
        if (IsInvincible) return; // 無敵時はダメージ無効

        currentHP -= damage;
        Debug.Log($"[BossHP] 現在HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            BossDie();
        }
    }

    // ボス死亡処理
    void BossDie()
    {
        skillSystem.TakeSkillPoint(pointValue);
        Destroy(gameObject);
    }

    // 無敵状態切り替え
    public void SetInvincible(bool invincible)
    {
        IsInvincible = invincible;
    }
}
