using UnityEngine;

//継承を想定して設計済み
public class PlayerBulletController : MonoBehaviour
{
    // 弾の速度
    public float baseDamage = 10f;
    protected float finalDamage;

    public virtual void Initialize(float playerAttackMultiplier)
    {
        finalDamage = baseDamage * playerAttackMultiplier;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zako"))
        {
            ZakoHP zakoHP = other.gameObject.GetComponent<ZakoHP>();
            if (zakoHP != null)
            {
                zakoHP.TakeDamage(finalDamage);
            }
            else
            {
                Debug.LogWarning($"オブジェクト '{other.gameObject.name}' にはZakoHPスクリプトがありません。");
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            BossHP bossHP = other.gameObject.GetComponent<BossHP>();
            if (bossHP != null)
            {
                bossHP.TakeDamage(finalDamage);
            }
            else
            {
                Debug.LogWarning($"オブジェクト '{other.gameObject.name}' にはBossHPスクリプトがありません。");
            }

            Destroy(gameObject);
        }
    }
}
