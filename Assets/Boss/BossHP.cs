using UnityEngine;
using System.Collections;

public class BossHP : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public SkillSystem skillSystem;
    public float maxHP = 100;
    private float currentHP;
    public int pointValue = 100;
    public int bossScore = 200000;

    private SpriteRenderer bossSpriteRenderer;

    // 無敵状態フラグ
    public bool IsInvincible { get; private set; } = false;
    public bool IsInvincibleByDeathEffect { get; private set; } = false;

    void Start()
    {
        currentHP = maxHP;

        bossSpriteRenderer = GetComponent<SpriteRenderer>();

        if (skillSystem == null)
        {
            skillSystem = FindFirstObjectByType<SkillSystem>(FindObjectsInactive.Include);

            if (skillSystem == null)
            {
                Debug.LogError("SkillSystem が見つかりません");
            }
        }
    }

    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;

    // ダメージを受ける関数
    public void TakeDamage(float damage)
    {
        if (IsInvincible || IsInvincibleByDeathEffect) return; // 無敵時はダメージ無効

        currentHP -= damage;
        Debug.Log($"[BossHP] 現在HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            InformationUIController.Instance.UpdateScoreDisplay((int)(Mathf.CeilToInt((damage+currentHP)/4)));
            InformationUIController.Instance.UpdateBossHP(0f);
            StartCoroutine(BossDieCoroutine());
        }
        else
        {
            InformationUIController.Instance.UpdateScoreDisplay((int)(Mathf.CeilToInt(damage/4)));
            InformationUIController.Instance.UpdateBossHP(currentHP/maxHP);
        }
    }

    // ボス死亡処理
    public IEnumerator BossDieCoroutine()
    {
        skillSystem.TakeSkillPoint(pointValue);
        InformationUIController.Instance.UpdateScoreDisplay(bossScore);
        IsInvincibleByDeathEffect = true;

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.StartBossDeathEffect();
        }

        //ここに演出を入れる
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomOffset = (Vector3)Random.insideUnitCircle * 2f;
            Instantiate(explosionPrefab, transform.position + randomOffset, Quaternion.identity);
            SoundManager.Instance.PlaySound(explosionSound, 0.5f);
            yield return new WaitForSeconds(0.3f);
        }

        DeleteAllEnemyBullets();
        bossSpriteRenderer.enabled = false;
        player.StartExit();
        
        yield return new WaitForSeconds(3f);

        //演出はここまで

        if (player != null)
        {
            player.OnBossDefeated();
        }
        EnemyWaveManager manager = FindAnyObjectByType<EnemyWaveManager>();
        if (manager != null)
        {
            manager.OnBossDefeated();
        }
        
        
        Destroy(gameObject);
    }

    public void DeleteAllEnemyBullets()
    {
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

        foreach (GameObject bullet in enemyBullets)
        {
            Destroy(bullet);
        }
    }

    // 無敵状態切り替え
    public void SetInvincible(bool invincible)
    {
        IsInvincible = invincible;
    }
}
