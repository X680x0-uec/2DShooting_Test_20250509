using UnityEngine;

public class BurstPlayerBulletController : PlayerBulletController
{
    public GameObject burstedBulletPrefab;
    public int wayCount = 16;
    public float randomOffsetRange = 0.5f;
    private float startAngle = -180f;
    private float attackMultiplier;

    public override void Initialize(float playerAttackMultiplier)
    {
        base.Initialize(playerAttackMultiplier);
        attackMultiplier = playerAttackMultiplier;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
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
            Burst();

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
            Burst();

            Destroy(gameObject);
        }
    }

    private void Burst()
    {
        float angleStep = 360f / wayCount;

        for (int i = 0; i < wayCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);

            Vector3 randomOffset = new Vector3(
                Random.Range(-randomOffsetRange, randomOffsetRange),
                Random.Range(-randomOffsetRange, randomOffsetRange),
                0f
            );

            Quaternion bulletRotation = transform.rotation * Quaternion.Euler(0, 0, currentAngle);

            GameObject newBullet = Instantiate(burstedBulletPrefab, transform.position + randomOffset, bulletRotation);

            PlayerBulletController bulletScript = newBullet.GetComponent<PlayerBulletController>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(attackMultiplier);
            }
        }
    }
}
