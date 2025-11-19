using UnityEngine;

public class AreaDamageExplosion : MonoBehaviour
{
    public GameObject debrisSpawnerPrefab;
    public float blastRadius = 3f;
    public float damageToDeal = 0f;

    public void SetDamage(float damage)
    {
        damageToDeal = damage;
    }

    void Start()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, blastRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Zako"))
            {
                ZakoHP zakoHP = hit.GetComponent<ZakoHP>();
                if (zakoHP != null)
                {
                    zakoHP.TakeDamage(damageToDeal);
                }
            }
            else if (hit.CompareTag("Boss"))
            {
                BossHP bossHP = hit.GetComponent<BossHP>();
                if (bossHP != null)
                {
                    bossHP.TakeDamage(damageToDeal);
                }
            }
            else if (hit.CompareTag("EnemyBullet"))
            {
                Instantiate(debrisSpawnerPrefab, hit.transform.position, Quaternion.identity);
                Destroy(hit.gameObject);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
