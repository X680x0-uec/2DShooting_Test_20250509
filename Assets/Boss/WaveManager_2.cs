using UnityEngine;
using System.Collections;

public class WaveManager_2 : MonoBehaviour
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

    [Header("参照")]
    [SerializeField] private BossHP bossHP;
    [SerializeField] private BossEnterMovement bossEnter;

    [Header("Wave設定")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private AttackConfig attackConfig; // 任意参照

    [Header("Wave移行設定")]
    [SerializeField] private float waveTransitionDelay = 3f;
    [SerializeField] private float postInvincibleDelay = 0.5f;

    [Header("SE")]
    [SerializeField] private AudioClip roarSE;

    private int currentWaveIndex = 0;
    private bool isTransitioning = false;
    private bool isBossDead = false;
    private bool hasStartedWaves = false;

    private void Start()
    {
        if (bossHP == null) bossHP = FindFirstObjectByType<BossHP>();
        if (bossHP == null) { Debug.LogError("[WaveManager] BossHP が見つかりません。"); enabled = false; return; }

        if (bossEnter == null) bossEnter = FindFirstObjectByType<BossEnterMovement>();
        if (bossEnter == null) { Debug.LogError("[WaveManager] BossEnterMovement が見つかりません。"); enabled = false; return; }

        bossHP.OnBossDead += OnBossDead;
        bossEnter.OnEnterFinished += OnBossEnterFinished;

        DisableAllAttackPatterns();
        ApplyAttackConfig();
    }

    private void ApplyAttackConfig()
    {
        if (attackConfig == null) return;

        foreach (var wave in waves)
        {
            foreach (var pattern in wave.attackPatterns)
            {
                if (pattern == null) continue;

                // BeamAttack
                var beam = pattern as BeamAttack;
                if (beam != null)
                {
                    typeof(BeamAttack).GetField("fireInterval", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(beam, attackConfig.beamFireInterval);
                    typeof(BeamAttack).GetField("bulletSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(beam, attackConfig.beamBulletSpeed);
                    typeof(BeamAttack).GetField("fireYRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(beam, attackConfig.beamFireYRange);
                }

                // SplitAttack
                var split = pattern as SplitAttack;
                if (split != null)
                {
                    typeof(SplitAttack).GetField("fireInterval", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(split, attackConfig.splitFireInterval);
                }

                // ParentBullet
                var parent = pattern as ParentBullet;
                if (parent != null)
                {
                    parent.childBulletCount = attackConfig.parentChildCount;
                    parent.childSpreadAngle = attackConfig.parentChildSpreadAngle;
                    parent.speed = attackConfig.parentSpeed;
                    parent.decelerationRate = attackConfig.parentDecelerationRate;
                    parent.splitTime = attackConfig.parentSplitTime;
                    parent.destroyDelay = attackConfig.parentDestroyDelay;
                    parent.splitSE = attackConfig.parentSplitSE;
                }

                // ChildBullet
                var child = pattern as ChildBullet;
                if (child != null)
                {
                    child.speed = attackConfig.childSpeed;
                    child.lifetime = attackConfig.childLifetime;
                }

                // SineWaveAttack
                var sine = pattern as SineWaveAttack;
                if (sine != null)
                {
                    typeof(SineWaveAttack).GetField("fireInterval", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(sine, attackConfig.sineFireInterval);
                    typeof(SineWaveAttack).GetField("bulletSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(sine, attackConfig.sineBulletSpeed);
                    typeof(SineWaveAttack).GetField("amplitude", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(sine, attackConfig.sineAmplitude);
                    typeof(SineWaveAttack).GetField("frequency", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(sine, attackConfig.sineFrequency);
                    typeof(SineWaveAttack).GetField("fireRangeY", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(sine, attackConfig.sineFireRangeY);
                }
            }
        }
    }

    private void OnBossEnterFinished() => StartCoroutine(StartWavesAfterDelay(0.5f));

    private IEnumerator StartWavesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (roarSE != null) SoundManager.Instance.PlaySound(roarSE, 0.25f);
        bossHP.SetInvincible(false);
        ActivateWave(0);
        hasStartedWaves = true;
    }

    private void Update()
    {
        if (!hasStartedWaves || isTransitioning || isBossDead) return;
        if (bossHP == null || bossHP.MaxHP <= 0) return;
        if (currentWaveIndex >= waves.Length - 1) return;

        float hpRate = bossHP.CurrentHP / bossHP.MaxHP;
        if (hpRate <= waves[currentWaveIndex].nextWaveThreshold)
        {
            StartCoroutine(TransitionToNextWave());
        }
    }

    private IEnumerator TransitionToNextWave()
    {
        isTransitioning = true;

        if (roarSE != null) SoundManager.Instance.PlaySound(roarSE, 0.25f);

        DisableWave(currentWaveIndex);
        bossHP.SetInvincible(true);

        BossFloating floating = bossHP.GetComponent<BossFloating>();
        if (floating != null) floating.SetPaused(true);

        SpriteRenderer sr = bossHP.GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        bool visible = true;
        float blinkInterval = 0.2f;

        while (elapsed < waveTransitionDelay)
        {
            if (isBossDead) yield break;
            if (sr) sr.enabled = visible;
            visible = !visible;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        if (sr) sr.enabled = true;

        bossHP.SetInvincible(false);
        if (floating != null) floating.SetPaused(false);

        yield return new WaitForSeconds(postInvincibleDelay);

        ActivateWave(currentWaveIndex + 1);
        isTransitioning = false;
    }

    private void ActivateWave(int index)
    {
        if (index < 0 || index >= waves.Length) return;
        currentWaveIndex = index;
        foreach (var pattern in waves[currentWaveIndex].attackPatterns)
        {
            if (pattern != null) pattern.enabled = true;
        }
    }

    private void DisableWave(int index)
    {
        if (index < 0 || index >= waves.Length) return;
        foreach (var pattern in waves[index].attackPatterns)
        {
            if (pattern != null) pattern.enabled = false;
        }
    }

    private void DisableAllAttackPatterns()
    {
        foreach (var wave in waves)
        {
            foreach (var pattern in wave.attackPatterns)
            {
                if (pattern != null) pattern.enabled = false;
            }
        }
    }

    private void OnBossDead()
    {
        isBossDead = true;
        isTransitioning = false;
        DisableWave(currentWaveIndex);
        BossFloating floating = bossHP.GetComponent<BossFloating>();
        if (floating != null) floating.SetPaused(true);
    }
}
