using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InformationUIController : MonoBehaviour
{
    public PlayerController playerController;

    public GameObject lifeIconPrefab;
    public RectTransform lifeIconsParent;
    public GameObject specialIconPrefab;
    public RectTransform specialIconsParent;
    private List<GameObject> activeLifeIcons = new List<GameObject>();
    private List<GameObject> activeSpecialIcons = new List<GameObject>();

    public TextMeshProUGUI scoreText;

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
        UpdateSpecialsDisplay(playerController.supecialSkillUsesLeft);
        UpdateScoreDisplay(0);
        ShowBossHP(false, 0f);
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

    public void UpdateSpecialsDisplay(int currentSpecials)
    {
        foreach (GameObject icon in activeSpecialIcons)
        {
            Destroy(icon);
        }
        activeSpecialIcons.Clear();

        for (int i = 0; i < currentSpecials; i++)
        {
            GameObject newIcon = Instantiate(specialIconPrefab, specialIconsParent);
            activeSpecialIcons.Add(newIcon);
        }
    }

    public void UpdateScoreDisplay(int currentScore)
    {
        scoreText.text = $"Score: {currentScore:D7}";
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
