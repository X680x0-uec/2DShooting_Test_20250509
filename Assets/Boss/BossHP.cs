using UnityEngine;

public class BossHP : MonoBehaviour
{
    public SkillSystem skillSystem;
    public float maxHP = 100;
    private float currentHP;
    public int pointValue = 100;
    void Start()
    {
        currentHP = maxHP;

        if (skillSystem == null)
        {
            skillSystem = FindFirstObjectByType<SkillSystem>(FindObjectsInactive.Include);

            if (skillSystem == null)
            {
                Debug.LogError("SkillSystem が見つかりません");
            }
        }
    }

    // ダメージを受ける関数
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("敵の現在HP: " + currentHP);

        if (currentHP <= 0)
        {
            BossDie();
        }
    }

    // 敵が死ぬときの処理
    void BossDie()
    {
        skillSystem.TakeSkillPoint(pointValue);
        Destroy(gameObject);
    }
}