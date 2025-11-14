using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public enum SpecialSkillType
{
    Empty,
    Laser,
    Vanish,
    Invincible
}

public enum PassiveAbility
{
    FastAttack,
    Revenge
}

public enum AttackBuffSource
{
    Normal,
    FastAttack,
    Revenge
}

public class PlayerController : MonoBehaviour
{
    [Header("管理情報")]
    public RankingScreenController rankingScreen;
    public static bool IsGameOverOrGameClear { get; private set; } = false;
    public int maxStageNumber = 1;
    private int stageNumber = 1;

    [Header("効果音")]
    public AudioClip appearSound;
    public AudioClip explosionByGameOverSound;
    public AudioClip damage1Sound;
    public AudioClip damage2Sound;
    public AudioClip cannotUseSound;
    public AudioClip vanishSound;
    public AudioClip laserSound;
    public AudioClip invincibleSound;

    [Header("ビジュアル")]
    public float screenEdgeDistance = 0.3f;
    public float uiAreaHeight = 2f;
    public Slider specialCountdownBar;
    public SpriteRenderer hitboxSpriteRenderer;
    public CircleCollider2D hitboxCollider;
    public float blinkInterval = 0.1f; //点滅間隔
    public GameObject explosionPrefab;
    private SpriteRenderer playerSpriteRenderer;
    private Color originalPlayerColor;
    private Vector2 screenBounds; //画面端の位置
    private Coroutine energyCountUpCoroutine;

    [Header("登場演出")]
    public float entryTargetX = -6f;
    public float entrySmoothTime = 0.5f;
    private bool isEntering = true;
    private Vector3 entryVelocity = Vector3.zero;
    private bool isInsideScreenEntering = false;

    [Header("自機弾関連")]
    //メインショット
    public GameObject normalMainOptionPrefab;

    //サブショット
    public GameObject normalOptionPrefab;
    public GameObject homingOptionPrefab;
    public GameObject spreadOptionPrefab;
    public GameObject burstOptionPrefab;
    public GameObject orbitOptionPrefab;

    //その他プレハブ
    public Transform orbitPivot;

    public List<FirePointController> firePoints = new List<FirePointController>(); //発射地点のリスト

    [Header("基本パラメータ")]
    public float moveSpeed = 10f;
    public float slowMoveSpeed = 3f;
    private Dictionary<AttackBuffSource, float> _attackMultipliers = new Dictionary<AttackBuffSource, float>();
    public int life = 5;
    public SpecialSkillType currentSkill = SpecialSkillType.Empty; //セット中のスキル
    private Dictionary<SpecialSkillType, int> skillEnergyCosts;
    public HashSet<PassiveAbility> passiveAbilities = new HashSet<PassiveAbility>();
    public int maxSpecialSkillEnergy = 30;
    public int specialSkillEnergy = 0;
    private bool isUsingSpecialSkill = false;
    private bool isInvincible = false;
    public float invincibilityDuration = 2f; //被弾時無敵の長さ
    private bool isControllLocked = false;
    public float controlLockDuration = 0.2f; //被弾時操作不能時間の長さ
    private bool isNoHit = true;

    [Header("特殊スキル用")]
    public GameObject laserPrefab;
    public float laserDuration = 3f;
    public float laserDurationMultiplier = 1f;
    public GameObject debrisSpawnerPrefab;
    public float vanishDamage = 1000f;
    public float vanishShakeDuration = 0.5f;
    public float vanishShakeMagnitude = 0.1f;
    private CameraShaker cameraShaker;
    public float invincibleSkillDuration = 5f;
    public float invincibleSkillDurationMultiplier = 1f;
    public bool IsInvincibleBySpecialSkill { get; private set; } = false;

    [Header("パッシブアビリティ用")]
    public float fastAttackMultiplier = 1.3f;
    public float revengeAttackMultiplier = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //端の座標を取得
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        
        Transform visualChild = transform.Find("Visual");
        if (visualChild != null)
        {
            playerSpriteRenderer = visualChild.GetComponent<SpriteRenderer>();
            if (playerSpriteRenderer != null)
            {
                originalPlayerColor = playerSpriteRenderer.color;
            }
        }
        else
        {
            Debug.LogWarning("Playerの子オブジェクトに'Visual'が見つかりません。");
        }

        //カメラにアクセス
        if (Application.isPlaying)
        {
            cameraShaker = CameraShaker.Instance;
            if (cameraShaker == null)
            {
                Debug.LogWarning("CameraShaker.Instance が見つかりませんでした");
            }
        }

        if (specialCountdownBar != null)
        {
            specialCountdownBar.gameObject.SetActive(false);
        }

        //各スキルの消費コストを設定
        skillEnergyCosts = new Dictionary<SpecialSkillType, int>();
        skillEnergyCosts.Add(SpecialSkillType.Laser, 10);
        skillEnergyCosts.Add(SpecialSkillType.Vanish, 6);
        skillEnergyCosts.Add(SpecialSkillType.Invincible, 7);

        specialSkillEnergy = maxSpecialSkillEnergy;
        InformationUIController.Instance.InitializeEnergyGauge(maxSpecialSkillEnergy);
        InformationUIController.Instance.UpdateEnergyDisplay(specialSkillEnergy, skillEnergyCosts[currentSkill]);

        //登場演出用
        transform.position = new Vector3(-screenBounds.x - 2f, 0, 0);
        isEntering = true;

        //初期メインショット
        AddOption(normalMainOptionPrefab, new Vector2(0f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //任意の処理を割り込ませる用(Pキー)
            GetSkill("Junior", 1, 1);
            GetSkill("Junior", 3, 1);
            Debug.Log("pressed P");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //任意の処理を割り込ませる用(Lキー)
            InformationUIController.Instance.UpdateScoreDisplay(100000);
            Debug.Log("pressed L");
        }
        if (isEntering) //登場処理
        {
            Vector3 targetPosition = new Vector3(entryTargetX, transform.position.y, transform.position.z);

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref entryVelocity, entrySmoothTime);
            if (!isInsideScreenEntering && transform.position.x > -screenBounds.x)
            {
                SoundManager.Instance.PlaySound(appearSound);
                isInsideScreenEntering = true;
            }
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isEntering = false;
            }
            return;
        }
        if (!isControllLocked && !IsGameOverOrGameClear && !SkillSystemOnOff.IsCheckingSkill) //通常時にする処理(操作等)
        {
            //低速移動
            float currentSpeed;
            if (Input.GetButton("Slow"))
            {
                currentSpeed = slowMoveSpeed;
                if (passiveAbilities.Contains(PassiveAbility.FastAttack))
                {
                    _attackMultipliers.Remove(AttackBuffSource.FastAttack);
                }
            }
            else
            {
                currentSpeed = moveSpeed;
                if (passiveAbilities.Contains(PassiveAbility.FastAttack))
                {
                    _attackMultipliers[AttackBuffSource.FastAttack] = fastAttackMultiplier;
                }
            }
            // 左右入力の取得
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            //位置計算
            Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
            Vector3 newPosition = transform.position + (Vector3)(moveDirection * currentSpeed * Time.deltaTime);

            newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x + screenEdgeDistance, screenBounds.x - screenEdgeDistance);
            newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y + uiAreaHeight + screenEdgeDistance, screenBounds.y - screenEdgeDistance);

            transform.position = newPosition;

            //弾の発射
            if (Input.GetButton("Shoot"))
            {
                Shoot();
            }

            if (currentSkill == SpecialSkillType.Empty)
            {
                SoundManager.Instance.PlaySound(cannotUseSound);
            }
            //スキル発動
            else if (Input.GetButtonDown("Special") && !isUsingSpecialSkill)
            {
                UseSpecialSkill();
            }
        }
    }

    void Shoot()
    {
        foreach (FirePointController point in firePoints)
        {
            point.Fire();
        }
    }

    public void GetSkill(string category, int id, int level)
    {
        switch (category)
        {
            case "Special":
                switch (id)
                {
                    case 0:
                        switch (level)
                        {
                            case 1:
                                ChangeSpecialSkill(SpecialSkillType.Laser);
                                break;
                            case 2:
                                laserDurationMultiplier = 1.2f;
                                break;
                            case 3:
                                laserDurationMultiplier = 1.5f;
                                break;
                        }
                        break;
                    case 1:
                        switch (level)
                        {
                            case 1:
                                ChangeSpecialSkill(SpecialSkillType.Vanish);
                                break;
                        }
                        break;
                    case 2:
                        switch (level)
                        {
                            case 1:
                                ChangeSpecialSkill(SpecialSkillType.Invincible);
                                break;
                            case 2:
                                invincibleSkillDurationMultiplier = 1.4f;
                                break;
                            case 3:
                                invincibleSkillDurationMultiplier = 2f;
                                break;
                        }
                        break;
                }
                break;
            case "HP":
                switch (level)
                {
                    case 1:
                        life += 1;
                        InformationUIController.Instance.UpdateLivesDisplay(life);
                        if (passiveAbilities.Contains(PassiveAbility.Revenge))
                        {
                            _attackMultipliers.Remove(AttackBuffSource.Revenge);
                        }
                        break;
                    case 2:
                        life += 1;
                        InformationUIController.Instance.UpdateLivesDisplay(life);
                        if (passiveAbilities.Contains(PassiveAbility.Revenge))
                        {
                            _attackMultipliers.Remove(AttackBuffSource.Revenge);
                        }
                        break;
                    case 3:
                        life += 1;
                        InformationUIController.Instance.UpdateLivesDisplay(life);
                        if (passiveAbilities.Contains(PassiveAbility.Revenge))
                        {
                            _attackMultipliers.Remove(AttackBuffSource.Revenge);
                        }
                        break;
                }
                break;
            case "Attack":
                switch (level)
                {
                    case 1:
                        _attackMultipliers[AttackBuffSource.Normal] = 1.2f;
                        break;
                    case 2:
                        _attackMultipliers[AttackBuffSource.Normal] = 1.5f;
                        break;
                    case 3:
                        _attackMultipliers[AttackBuffSource.Normal] = 2f;
                        break;
                }
                break;
            case "Passive":
                switch (id)
                {
                    case 0:
                        passiveAbilities.Add(PassiveAbility.FastAttack);
                        break;
                    case 1:
                        passiveAbilities.Add(PassiveAbility.Revenge);
                        CheckAndChangeToRevengeMode();
                        break;
                }
                break;
            case "Shot":
                switch (id)
                {
                    case 0:
                        switch (level)
                        {
                            case 1:
                                ChangeOptionFireRateMultiplierByTag("MainOption", 0.7f);
                                break;
                            case 2:
                                ChangeOptionFireRateMultiplierByTag("MainOption", 0.5f);
                                break;
                            case 3:
                                ChangeOptionFireRateMultiplierByTag("MainOption", 0.3f);
                                break;
                        }
                        break;
                }
                break;
            case "Junior":
                switch (id)
                {
                    case 0:
                        switch (level)
                        {
                            case 1:
                                AddOption(normalOptionPrefab, new Vector2(1.0f, 0.4f));
                                AddOption(normalOptionPrefab, new Vector2(1.0f, -0.4f));
                                break;
                        }
                        break;
                    case 1:
                        switch (level)
                        {
                            case 1:
                                AddOption(homingOptionPrefab, new Vector2(-1.0f, 0.6f));
                                AddOption(homingOptionPrefab, new Vector2(-1.0f, -0.6f));
                                break;
                        }
                        break;
                    case 2:
                        switch (level)
                        {
                            case 1:
                                AddOption(spreadOptionPrefab, new Vector2(0f, 0.8f));
                                AddOption(spreadOptionPrefab, new Vector2(0f, -0.8f));
                                break;
                            case 2:
                                ChangeSpreadOption(5, 50);
                                break;
                        }
                        break;
                    case 3:
                        switch (level)
                        {
                            case 1:
                                AddOption(burstOptionPrefab, new Vector2(1f, 0f));
                                break;
                        }
                        break;
                    case 4:
                        switch (level)
                        {
                            case 1:
                                AddOption(orbitOptionPrefab, new Vector2(1.5f, 0f), orbitPivot);
                                AddOption(orbitOptionPrefab, new Vector2(-1.5f, 0f), orbitPivot);
                                break;
                        }
                        break;
                }
                break;
        }
    }

    public void ChangeSpecialSkill(SpecialSkillType newSpecialSkillType)
    {
        currentSkill = newSpecialSkillType;
        InformationUIController.Instance.UpdateEnergyDisplay(specialSkillEnergy, skillEnergyCosts[currentSkill]);
    }

    public void AddOption(GameObject optionPrefab, Vector2 relativePosition, Transform parentTransform = null)
    {
        if (parentTransform == null)
        {
            parentTransform = this.transform;
        }
        Vector3 spawnPosition = transform.position + (Vector3)relativePosition;
        GameObject newOption = Instantiate(optionPrefab, spawnPosition, transform.rotation);
        newOption.transform.SetParent(parentTransform);

        FirePointController newFirePoint = newOption.GetComponent<FirePointController>();
        if (newFirePoint != null)
        {
            firePoints.Add(newFirePoint);
        }
    }

    public void RemoveOption(FirePointController optionToRemove)
    {
        if (firePoints.Contains(optionToRemove))
        {
            firePoints.Remove(optionToRemove);

            Destroy(optionToRemove.gameObject);

            Debug.Log($"子機 {optionToRemove.name} を削除しました。");
        }
    }

    public void RemoveAllOptions()
    {
        for (int i = firePoints.Count - 1; i >= 0; i--)
        {
            if (firePoints[i] != null)
            {
                Destroy(firePoints[i].gameObject);
            }
        }

        firePoints.Clear();
        Debug.Log("子機を全消去しました。");
    }

    public void ChangeOptionFireRateMultiplierByTag(string tagName, float rate)
    {
        foreach (FirePointController point in firePoints)
        {
            if (point.CompareTag(tagName))
            {
                point.SetFireRateMultiplier(rate);
            }
        }
    }

    public void ChangeSpreadOption(int way, float angle)
    {
        foreach (FirePointController point in firePoints)
        {
            if (point.CompareTag("SpreadOption"))
            {
                point.SetSpreadSetting(way, angle);
            }
        }
    }

    public float GetAttackMultiplier()
    {
        float finalMultiplier = _attackMultipliers.Values.Aggregate(1.0f, (acc, val) => acc * val);
        return finalMultiplier;
    }

    public void CheckAndChangeToRevengeMode()
    {
        if (life == 0 && passiveAbilities.Contains(PassiveAbility.Revenge))
        {
            _attackMultipliers[AttackBuffSource.Revenge] = revengeAttackMultiplier;
            Debug.Log("リベンジモードになりました");
        }
    }

    void UseSpecialSkill()
    {
        int cost = skillEnergyCosts[currentSkill];

        if (specialSkillEnergy >= cost)
        {
            if (energyCountUpCoroutine != null)
            {
                StopCoroutine(energyCountUpCoroutine);
            }
            isUsingSpecialSkill = true;
            specialSkillEnergy -= cost;
            InformationUIController.Instance.UpdateEnergyDisplay(specialSkillEnergy, cost);

            switch (currentSkill)
            {
                case SpecialSkillType.Laser:
                    StartCoroutine(LaserCoroutine());
                    break;
                case SpecialSkillType.Vanish:
                    StartCoroutine(VanishCoroutine());
                    break;
                case SpecialSkillType.Invincible:
                    StartCoroutine(InvincibleSkillCoroutine());
                    break;
            }
        }
        else
        {
            SoundManager.Instance.PlaySound(cannotUseSound);
            Debug.Log("エネルギー不足");
        }
    }

    public void ApplyDamage()
    {
        StartBlinkEffect();
        if (isNoHit)
        {
            isNoHit = false;
        }
        if (life <= 0)
        {
            Gameover();
        }
        else
        {
            life -= 1;
            SoundManager.Instance.PlaySound(damage1Sound);
            CheckAndChangeToRevengeMode();
            InformationUIController.Instance.UpdateLivesDisplay(life);

            energyCountUpCoroutine = StartCoroutine(SpecialSkillEnergyCountUpCoroutine(specialSkillEnergy));
            specialSkillEnergy = maxSpecialSkillEnergy;
        }
    }

    private IEnumerator SpecialSkillEnergyCountUpCoroutine(int currentEnergy)
    {
        int countingUpEnergy = currentEnergy;
        while (countingUpEnergy < maxSpecialSkillEnergy)
        {
            countingUpEnergy++;
            InformationUIController.Instance.UpdateEnergyDisplay(countingUpEnergy, skillEnergyCosts[currentSkill]);
            yield return new WaitForSeconds(0.05f);
        }
        InformationUIController.Instance.UpdateEnergyDisplay(specialSkillEnergy, skillEnergyCosts[currentSkill]);
        
        energyCountUpCoroutine = null;
    }

    private IEnumerator LaserCoroutine()
    {
        SoundManager.Instance.PlaySound(laserSound);
        Vector3 laserPosition = transform.position + new Vector3(8.9f, 0, 0);
        GameObject laserObject = Instantiate(laserPrefab, laserPosition, Quaternion.identity);

        laserObject.transform.SetParent(this.transform); //レーザーをプレイヤーの子オブジェクト化

        Laser laserScript = laserObject.GetComponent<Laser>();
        if (laserScript == null)
        {
            Debug.LogError("Laserスクリプトにアクセスできません");
            Destroy(laserObject);
            isUsingSpecialSkill = false;
            yield break;
        }

        if (specialCountdownBar != null)
        {
            specialCountdownBar.gameObject.SetActive(true);
            specialCountdownBar.value = 1f;
        }

        float maxLaserDuration = laserDuration * laserDurationMultiplier;
        float elapsedLaserDuration = 0f;

        while (elapsedLaserDuration < maxLaserDuration)
        {
            float ratio = 1.0f - (elapsedLaserDuration / maxLaserDuration);
            if (specialCountdownBar != null)
            {
                specialCountdownBar.value = ratio;
            }

            elapsedLaserDuration += Time.deltaTime;
            yield return null;
        }

        if (specialCountdownBar != null)
        {
            specialCountdownBar.gameObject.SetActive(false);
        }
        laserScript.TriggerEndAnimation();
        isUsingSpecialSkill = false;
    }

    private IEnumerator VanishCoroutine()
    {
        SoundManager.Instance.PlaySound(vanishSound);
        if (cameraShaker != null)
        {
            cameraShaker.ShakeCamera(vanishShakeDuration, vanishShakeMagnitude);
        }

        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject bullet in enemyBullets)
        {
            Instantiate(debrisSpawnerPrefab, bullet.transform.position, Quaternion.identity);
            Destroy(bullet);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Zako");
        foreach (GameObject enemy in enemies)
        {
            Instantiate(debrisSpawnerPrefab, enemy.transform.position, Quaternion.identity);

            ZakoHP zakoHP = enemy.GetComponent<ZakoHP>();
            if (zakoHP != null)
            {
                zakoHP.TakeDamage(vanishDamage);
            }
        }

        yield return new WaitForSeconds(vanishShakeDuration);

        isUsingSpecialSkill = false;
    }

    private IEnumerator InvincibleSkillCoroutine()
    {
        SoundManager.Instance.PlaySound(invincibleSound);
        if (specialCountdownBar != null)
        {
            specialCountdownBar.gameObject.SetActive(true);
            specialCountdownBar.value = 1f;
        }

        IsInvincibleBySpecialSkill = true;

        float maxInvincibleSkillDuration = invincibleSkillDuration * invincibleSkillDurationMultiplier;
        float elapsedInvincibleSkillDuration = 0f;
        float hue = 0f;

        while (elapsedInvincibleSkillDuration < maxInvincibleSkillDuration)
        {
            float ratio = 1.0f - (elapsedInvincibleSkillDuration / maxInvincibleSkillDuration);
            if (specialCountdownBar != null)
            {
                specialCountdownBar.value = ratio;
            }

            //色替え
            if (playerSpriteRenderer != null)
            {
                hue = (Time.time * 1f) % 1f;
                Color rainbowColor = Color.HSVToRGB(hue, 0.4f, 1f);
                playerSpriteRenderer.color = rainbowColor;
            }

            elapsedInvincibleSkillDuration += Time.deltaTime;
            yield return null;
        }

        if (specialCountdownBar != null)
        {
            specialCountdownBar.gameObject.SetActive(false);
        }
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.color = originalPlayerColor;
        }
        IsInvincibleBySpecialSkill = false;
        isUsingSpecialSkill = false;
    }

    public void StartBlinkEffect()
    {
        if (!isInvincible && playerSpriteRenderer != null) //無敵中でなければ開始
        {
            StartCoroutine(BlinkEffectCoroutine());
        }
    }

    private IEnumerator BlinkEffectCoroutine()
    {
        isInvincible = true;
        isControllLocked = true;
        // 当たり判定のコライダーを無効化
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }
        yield return new WaitForSeconds(controlLockDuration); //被弾時に一瞬動けなくする
        isControllLocked = false;
        float invincibleEndTime = Time.time + invincibilityDuration - controlLockDuration;
        SoundManager.Instance.PlaySound(damage2Sound);

        //無敵時間待機
        while (Time.time < invincibleEndTime)
        {
            playerSpriteRenderer.enabled = !playerSpriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        playerSpriteRenderer.enabled = true;
        isInvincible = false;
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true;
        }
    }

    public void StartBossDeathEffect()
    {
        hitboxCollider.enabled = false;
    }

    public void OnBossDefeated()
    {
        if (stageNumber >= maxStageNumber)
        {
            GameClear();
        }
        else
        {
            ChangeStage();
        }
    }

    private void GameClear()
    {
        IsGameOverOrGameClear = true;
        if (isNoHit)
        {
            StartCoroutine(DelayAppearingRankingScreenWithClear(InformationUIController.Instance.playerScore, "NO HIT CLEAR"));
        }
        else
        {
            StartCoroutine(DelayAppearingRankingScreenWithClear(InformationUIController.Instance.playerScore, "ALL CLEAR"));
        }
        Debug.Log("GameClear");
    }

    public IEnumerator DelayAppearingRankingScreenWithClear(int score, string progress)
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0;
        rankingScreen.ShowScreen(score, progress);
    }
    
    private void ChangeStage()
    {
        stageNumber += 1;
        hitboxCollider.enabled = true;
        transform.position = new Vector3(-screenBounds.x - 2f, 0, 0);
        isEntering = true;
        isInsideScreenEntering = false;
        Debug.Log("Stage changed");
    }

    public void Gameover()
    {
        IsGameOverOrGameClear = true;
        Time.timeScale = 0;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(explosionByGameOverSound, 0.7f);
        StartCoroutine(DelayAppearingRankingScreen(InformationUIController.Instance.playerScore, $"DIED IN STAGE{stageNumber}"));
        Debug.Log("Gameover");

        playerSpriteRenderer.enabled = false;
        hitboxSpriteRenderer.enabled = false;
        hitboxCollider.enabled = false;
        specialCountdownBar.gameObject.SetActive(false);
    }
    public IEnumerator DelayAppearingRankingScreen(int score, string progress)
    {
        yield return new WaitForSecondsRealtime(2f);
        rankingScreen.ShowScreen(score, progress);
    }
}
