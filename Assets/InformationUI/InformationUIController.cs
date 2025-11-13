using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InformationUIController : MonoBehaviour
{
    public PlayerController playerController;

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

    [Header("スコア")]
    public TextMeshProUGUI scoreText;
    public int playerScore = 0;
    private int currentDisplayedScore = 0;
    private Coroutine scoreCoroutine;

    [Header("スキルポイント")]
    public TextMeshProUGUI skillPointText;

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
        bool isShortage = currentEnergy < cost;

        for (int i = 0; i < energyIcons.Count; i++)
        {
            Image icon = energyIcons[i];

            if (i >= currentEnergy) //消費済みエネルギー
            {
                icon.sprite = emptyEnergySprite;
                icon.color = isShortage ? shortageEnergyColor : fullEnergyColor;
            }
            else if (i >= currentEnergy - cost) //次消費予定のエネルギー
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
        float duration = 0.2f;
        float elapsed = 0f;

        int startScore = playerScore;

        while (elapsed < duration)
        {
            float progress = elapsed / duration;

            int rollingScore = (int)Mathf.Lerp((float)startScore, (float)targetScore, progress);
            scoreText.text = $"Score: {rollingScore:D7}";

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        currentDisplayedScore = targetScore;
        scoreText.text = $"Score: {currentDisplayedScore:D7}";

        scoreCoroutine = null;
    }

    public void UpdateSkillPoint(int currentSkillPoint)
    {
        skillPointText.text = $"Skill Point: {currentSkillPoint:D5}";
    }

    public void ShowBossHP(bool show, float currentBossHealthRatio = 0f)
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
