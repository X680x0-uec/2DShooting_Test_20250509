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
    [SerializeField] private float waveTransitionDelay = 3f;   // 点滅＋無敵時間
    [SerializeField] private float postInvincibleDelay = 0.5f; // 無敵終了後の一呼吸

    private int currentWaveIndex = 0;
    private bool isTransitioning = false;

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

        // 最初のWaveを開始
        ActivateWave(currentWaveIndex);
    }

    private void Update()
    {
        if (bossHP == null || bossHP.MaxHP <= 0) return;
        if (currentWaveIndex >= waves.Length - 1) return; // 次のWaveが無ければ終了
        if (isTransitioning) return;

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

        // 現在のWave停止
        if (currentWaveIndex >= 0)
        {
            foreach (var pattern in waves[currentWaveIndex].attackPatterns)
            {
                if (pattern != null)
                {
                    pattern.enabled = false;
                    Debug.Log($"[WaveManager] Disabling {pattern.GetType().Name}");
                }
            }
        }

        // 無敵状態ON
        bossHP.SetInvincible(true);

        // BossFloatingを停止
        BossFloating floating = bossHP.GetComponent<BossFloating>();
        if (floating != null)
            floating.SetPaused(true);

        // 点滅処理
        SpriteRenderer sr = bossHP.GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        bool visible = true;
        float blinkInterval = 0.2f;

        while (elapsed < waveTransitionDelay)
        {
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

        // BossFloatingを再開
        if (floating != null)
            floating.SetPaused(false);

        // 無敵終了後の一呼吸
        yield return new WaitForSeconds(postInvincibleDelay);

        // 次のWave開始
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
            {
                pattern.enabled = true;
                Debug.Log($"[WaveManager] Enabling {pattern.GetType().Name}");
            }
        }
    }
}
