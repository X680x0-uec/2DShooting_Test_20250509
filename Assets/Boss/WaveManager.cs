using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        [Tooltip("このウェーブで有効にする攻撃スクリプトたち")]
        public MonoBehaviour[] attackPatterns;

        [Tooltip("このHP割合以下になったら次のWaveへ移行（0〜1）")]
        [Range(0f, 1f)]
        public float nextWaveThreshold = 0.7f;
    }

    [SerializeField] private BossHP bossHP;
    [SerializeField] private Wave[] waves;
    [SerializeField] private float waveTransitionDelay = 3f;
    [SerializeField] private float postInvincibleDelay = 0.5f;
    [SerializeField] private AudioClip roarSE;

    private int currentWaveIndex = 0;
    private bool isTransitioning = false;
    private bool isBossDead = false;

    private void Start()
    {
        if (bossHP == null)
        {
            bossHP = FindFirstObjectByType<BossHP>();
        }

        if (bossHP == null)
        {
            Debug.LogError("[WaveManager] BossHP が見つかりません。Bossにアタッチされていますか？");
            enabled = false;
            return;
        }

        // ★ ボス死亡イベントを受け取る
        bossHP.OnBossDead += OnBossDead;

        ActivateWave(currentWaveIndex);
    }

    private void Update()
    {
        if (isBossDead) return;               // ★ 死亡後は一切動かさない
        if (isTransitioning) return;
        if (bossHP == null || bossHP.MaxHP <= 0) return;
        if (currentWaveIndex >= waves.Length - 1) return;

        float hpRate = bossHP.CurrentHP / bossHP.MaxHP;
        float nextThreshold = waves[currentWaveIndex].nextWaveThreshold;

        if (hpRate <= nextThreshold)
        {
            StartCoroutine(TransitionToNextWave());
        }
    }

    private IEnumerator TransitionToNextWave()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        Debug.Log($"[WaveManager] Wave {currentWaveIndex + 1} 終了。{waveTransitionDelay}秒インターバルに入ります…");

        // 鳴き声SE
        if (roarSE != null)
            SoundManager.Instance.PlaySound(roarSE, 0.3f);

        // 現在のWave停止
        foreach (var pattern in waves[currentWaveIndex].attackPatterns)
        {
            if (pattern != null)
                pattern.enabled = false;
        }

        // 無敵ON
        bossHP.SetInvincible(true);

        // BossFloating 停止
        BossFloating floating = bossHP.GetComponent<BossFloating>();
        if (floating != null) floating.SetPaused(true);

        // 点滅
        SpriteRenderer sr = bossHP.GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        bool visible = true;
        float blinkInterval = 0.2f;

        while (elapsed < waveTransitionDelay)
        {
            if (isBossDead) yield break;  // ★ 死亡した瞬間に即終了

            if (sr != null)
                sr.enabled = visible;

            visible = !visible;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        if (sr != null)
            sr.enabled = true;

        // 無敵解除
        bossHP.SetInvincible(false);

        // BossFloating再開
        if (floating != null)
            floating.SetPaused(false);

        // 一呼吸
        yield return new WaitForSeconds(postInvincibleDelay);

        // 次のWave
        ActivateWave(currentWaveIndex + 1);

        isTransitioning = false;
    }

    private void ActivateWave(int index)
    {
        if (index < 0 || index >= waves.Length)
        {
            Debug.LogWarning("[WaveManager] Wave index out of range!");
            return;
        }

        currentWaveIndex = index;
        var wave = waves[currentWaveIndex];

        Debug.Log($"[WaveManager] Wave {currentWaveIndex + 1} 開始！");

        foreach (var pattern in wave.attackPatterns)
        {
            if (pattern != null)
                pattern.enabled = true;
        }
    }

    // ★ ボス死亡時の処理
    private void OnBossDead()
    {
        Debug.Log("[WaveManager] ボス死亡を検知 → WaveManager停止処理");

        isBossDead = true;
        isTransitioning = false;

        // AttackPattern を全停止
        if (currentWaveIndex >= 0 && currentWaveIndex < waves.Length)
        {
            foreach (var pattern in waves[currentWaveIndex].attackPatterns)
            {
                if (pattern != null)
                    pattern.enabled = false;
            }
        }

        // BossFloating 停止
        BossFloating floating = bossHP.GetComponent<BossFloating>();
        if (floating != null)
            floating.SetPaused(true);
    }
}
