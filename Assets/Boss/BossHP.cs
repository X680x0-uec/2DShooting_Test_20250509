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

    // 無敵状態
    public bool IsInvincible { get; private set; } = false;
    public bool IsInvincibleByDeathEffect { get; private set; } = false;

    // ★ ボス死亡を外部に知らせるイベント（WaveManager用）
    public event System.Action OnBossDead;

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

    // ダメージ処理
    public void TakeDamage(float damage)
    {
        if (IsInvincible || IsInvincibleByDeathEffect) return;

        currentHP -= damage;
        Debug.Log($"[BossHP] 現在HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            // スコア更新
            InformationUIController.Instance.UpdateScoreDisplay(
                (int)(Mathf.CeilToInt((damage + currentHP) / 4))
            );
            InformationUIController.Instance.UpdateBossHP(0f);

            StartCoroutine(BossDieCoroutine());
        }
        else
        {
            InformationUIController.Instance.UpdateScoreDisplay(
                (int)(Mathf.CeilToInt(damage / 4))
            );
            InformationUIController.Instance.UpdateBossHP(currentHP / maxHP);
        }
    }

    // ボス死亡コルーチン
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

        // ★死亡演出（爆発を10回）
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomOffset = (Vector3)Random.insideUnitCircle * 2f;
            Instantiate(explosionPrefab, transform.position + randomOffset, Quaternion.identity);
            SoundManager.Instance.PlaySound(explosionSound, 0.5f);
            yield return new WaitForSeconds(0.3f);
        }

        DeleteAllEnemyBullets();
        // ★ Destroy の直前でイベントを発火（WaveManager で受け取れる）←弾消し直後に位置を変更
        OnBossDead?.Invoke();
        bossSpriteRenderer.enabled = false;
        player.StartExit();
        
        yield return new WaitForSeconds(3f);

        if (player != null)
        {
            player.OnBossDefeated();
        }
        EnemyWaveManager manager = FindAnyObjectByType<EnemyWaveManager>();
        if (manager != null)
        {
            manager.OnBossDefeated();
        }

        DeleteAllEnemyBullets(); //念のためもう一度弾消し

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

    // 無敵切り替え
    public void SetInvincible(bool invincible)
    {
        IsInvincible = invincible;
    }
}
