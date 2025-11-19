using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class InformationUIController : MonoBehaviour
{
    public PlayerController playerController;
    public RankingScreenController rankingScreen;

    [Header("ステージ開始時テキスト")]
    public TextMeshProUGUI stageAnnounceText;
    public float stageAnnounceDisplayDuration = 1f;
    public float stageAnnounceFadeDuration = 1.5f;
    private Coroutine stageTextCoroutine;

    [Header("ゲームクリア時テキスト")]
    public TextMeshProUGUI clearAnnounceText;
    public float clearAnnounceShowInterval = 1.2f;
    public int clearBonusScore = 1000000;
    public float skillPointBonusMultiplier = 0.0125f;
    public int noHitBonusScore = 3000000;
    public int noSPBonusScore = 2000000;

    [Header("ライフ")]
    public GameObject lifeIconPrefab;
    public RectTransform lifeIconsParent;
    private List<GameObject> activeLifeIcons = new List<GameObject>();

    [Header("エネルギー")]
    public GameObject energyIconPrefab;
    public RectTransform energyIconsParent;
    public Sprite fullEnergySprite;
    public Sprite emptyEnergySprite;
    public Color fullEnergyColor = Color.red;
    public Color pendingEnergyColor = Color.yellow;
    public Color shortageEnergyColor = Color.gray;
    private List<Image> energyIcons = new List<Image>();

    [Header("スキルアイコン")]
    public Image currentSkillIconImage;
    public List<SkillIconData> skillIconData;

    [Header("スコア")]
    public TextMeshProUGUI scoreText;
    public int playerScore = 0;
    private int currentDisplayedScore = 0;
    private Coroutine scoreCoroutine;

    [Header("スキルポイント")]
    public TextMeshProUGUI skillPointText;
    private int displayedSkillPoint = 0;

    [Header("ボスHPとタイトルロゴ")]
    public Slider bossHPSlider;
    public GameObject titleLogo;

    public static InformationUIController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateLivesDisplay(playerController.life);
        UpdateScoreDisplay(0);
        UpdateSkillPoint(0);
        ShowBossHP(false, 0f);
    }

    public void ShowStageAnnounce(string text)
    {
        if (stageAnnounceText == null)
        {
            return;
        }

        if (stageTextCoroutine != null)
        {
            StopCoroutine(stageTextCoroutine);
        }

        stageTextCoroutine = StartCoroutine(StageAnnounceCoroutine(text));
    }

    private IEnumerator StageAnnounceCoroutine(string text)
    {
        stageAnnounceText.text = text;
        stageAnnounceText.gameObject.SetActive(true);

        Color currentColor = stageAnnounceText.color;
        currentColor.a = 1f;
        stageAnnounceText.color = currentColor;

        yield return new WaitForSeconds(stageAnnounceDisplayDuration);

        float elapsed = 0f;
        while (elapsed < stageAnnounceFadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / stageAnnounceFadeDuration);

            currentColor.a = alpha;
            stageAnnounceText.color = currentColor;

            yield return null;
        }

        stageAnnounceText.gameObject.SetActive(false);
        stageTextCoroutine = null;
    }

    public void ShowClearAnnounce(bool isNoHit, bool isNoSP)
    {
        if (clearAnnounceText == null)
        {
            return;
        }

        StartCoroutine(ClearAnnounceCoroutine(isNoHit, isNoSP));
    }

    private IEnumerator ClearAnnounceCoroutine(bool isNoHit, bool isNoSP)
    {
        clearAnnounceText.gameObject.SetActive(true);
        StringBuilder sb = new StringBuilder();

        //クリアボーナス
        sb.AppendLine($"CLEAR BONUS:\n+{clearBonusScore}");
        UpdateScoreDisplay(clearBonusScore);
        clearAnnounceText.text = sb.ToString();

        yield return new WaitForSecondsRealtime(clearAnnounceShowInterval);

        //スキルポイントボーナス
        int skillpointBonusScore = (int)(displayedSkillPoint * displayedSkillPoint * skillPointBonusMultiplier);
        sb.AppendLine($"SKILL POINT BONUS:\n+{skillpointBonusScore}");
        UpdateScoreDisplay(skillpointBonusScore);
        clearAnnounceText.text = sb.ToString();

        yield return new WaitForSecondsRealtime(clearAnnounceShowInterval);

        //ノーヒットボーナス
        if (isNoHit)
        {
            sb.AppendLine($"NO HIT BONUS:\n+{noHitBonusScore}");
            UpdateScoreDisplay(noHitBonusScore);
        }
        else
        {
            sb.AppendLine("NO HIT BONUS:\n+0");
        }
        clearAnnounceText.text = sb.ToString();

        yield return new WaitForSecondsRealtime(clearAnnounceShowInterval);

        //ノースペシャルスキルボーナス
        if (isNoSP)
        {
            sb.AppendLine($"NO SP BONUS:\n+{noSPBonusScore}");
            UpdateScoreDisplay(noSPBonusScore);
        }
        else
        {
            sb.AppendLine("NO SP BONUS:\n+0");
        }
        clearAnnounceText.text = sb.ToString();

        yield return new WaitForSecondsRealtime(clearAnnounceShowInterval * 2);

        clearAnnounceText.gameObject.SetActive(false);
        if (isNoHit && isNoSP)
        {
            rankingScreen.ShowScreen(playerScore, "NO HIT&SP CLEAR");
        }
        else if (isNoHit)
        {
            rankingScreen.ShowScreen(playerScore, "NO HIT CLEAR");
        }
        else if (isNoSP)
        {
            rankingScreen.ShowScreen(playerScore, "NO SP CLEAR");
        }
        else
        {
            rankingScreen.ShowScreen(playerScore, "ALL CLEAR");
        }
    }

    public void InitializeEnergyGauge(int maxEnergy)
    {
        for (int i = 0; i < maxEnergy; i++)
        {
            GameObject iconObj = Instantiate(energyIconPrefab, energyIconsParent);
            Image iconImage = iconObj.GetComponent<Image>();
            energyIcons.Add(iconImage);
        }
    }

    public void UpdateLivesDisplay(int currentLives)
    {
        foreach (GameObject icon in activeLifeIcons)
        {
            Destroy(icon);
        }
        activeLifeIcons.Clear();

        for (int i = 0; i < currentLives; i++)
        {
            GameObject newIcon = Instantiate(lifeIconPrefab, lifeIconsParent);
            activeLifeIcons.Add(newIcon);
        }
    }

    public void UpdateEnergyDisplay(int currentEnergy, int cost)
    {
        bool isShortage = false;
        if (cost == -1)
        {
            isShortage = currentEnergy <= 0;
        }
        else
        {
            isShortage = currentEnergy < cost;
        }

        for (int i = 0; i < energyIcons.Count; i++)
        {
            Image icon = energyIcons[i];

            if (i >= currentEnergy) //消費済みエネルギー
            {
                icon.sprite = emptyEnergySprite;
                icon.color = isShortage ? shortageEnergyColor : fullEnergyColor;
            }
            else if (i >= currentEnergy - cost || cost == -1) //次消費予定のエネルギー
            {
                icon.sprite = fullEnergySprite;
                icon.color = isShortage ? shortageEnergyColor : pendingEnergyColor;
            }
            else //未消費エネルギー
            {
                icon.sprite = fullEnergySprite;
                icon.color = isShortage ? shortageEnergyColor : fullEnergyColor;
            }
        }
    }

    public void UpdateCurrentSkillIcon(SpecialSkillType currentSkill)
    {
        if (currentSkillIconImage == null)
        {
            return;
        }

        foreach (var data in skillIconData)
        {
            if (data.skillType == currentSkill)
            {
                currentSkillIconImage.sprite = data.iconSprite;
                return;
            }
        }

        Debug.LogWarning($"スキル'{currentSkill}'のアイコンが見つかりません");
    }

    public void UpdateScoreDisplay(int targetScore)
    {
        playerScore += targetScore;
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }
        scoreCoroutine = StartCoroutine(ScoreRollCoroutine(playerScore));
    }

    private IEnumerator ScoreRollCoroutine(int targetScore)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        int startScore = currentDisplayedScore;

        while (elapsed < duration)
        {
            float progress = elapsed / duration;

            currentDisplayedScore = (int)Mathf.Lerp((float)startScore, (float)targetScore, progress);
            scoreText.text = $"Score: {currentDisplayedScore:D8}";

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        currentDisplayedScore = targetScore;
        scoreText.text = $"Score: {currentDisplayedScore:D8}";

        scoreCoroutine = null;
    }

    public void UpdateSkillPoint(int currentSkillPoint)
    {
        displayedSkillPoint = currentSkillPoint;
        skillPointText.text = $"Skill Point: {displayedSkillPoint:D6}";
    }

    public void ShowBossHP(bool show, float currentBossHealthRatio = 1f)
    {
        bossHPSlider.gameObject.SetActive(show);
        titleLogo.SetActive(!show);
        if (show)
        {
            bossHPSlider.value = currentBossHealthRatio;
        }
    }

    public void UpdateBossHP(float currentBossHealthRatio)
    {
        bossHPSlider.value = currentBossHealthRatio;
    }
}

[System.Serializable]
public class SkillIconData
{
    public SpecialSkillType skillType;
    public Sprite iconSprite;
}