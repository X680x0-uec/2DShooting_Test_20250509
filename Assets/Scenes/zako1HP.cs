using UnityEngine;

public class ZakoHP : MonoBehaviour
{
    public SkillSystem skillSystem;
    public float maxHP = 100;
    private float currentHP;
    public int pointValue = 100;
    void Start()
    {
        currentHP = maxHP;
    }

    // ダメージを受ける関数
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("敵の現在HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // 敵が死ぬときの処理
    void Die()
    {
        skillSystem.TakeSkillPoint(pointValue);
        Destroy(gameObject);
    }
}