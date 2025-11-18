using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Collections;
using UnityEngine.SceneManagement;

public class RankingScreenController : MonoBehaviour
{
    [Header("UIオブジェクト類")]
    public NameInputController nameInputController;
    public TextMeshProUGUI currentNameText;
    public TextMeshProUGUI rankingListText;
    public GameObject yourScorePanel;
    public TextMeshProUGUI yourScoreText;

    [Header("シーン")]
    public string titleSceneName = "Title";

    [Header("スコア情報(外部から設定)")]
    private int finalScore;
    private string finalProgress;

    private bool isRegistrationComplete = false;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isRegistrationComplete && Input.GetButtonDown("Submit"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(titleSceneName);
        }
    }

    public void ShowScreen(int score, string progress)
    {
        gameObject.SetActive(true);
        finalScore = score;
        finalProgress = progress;

        bool isRanked = RankingManager.Instance.CheckIfRanked(finalScore);

        if (isRanked)
        {
            isRegistrationComplete = false;
            nameInputController.gameObject.SetActive(true);
            nameInputController.rankingScreen = this;
            nameInputController.currentNameText = currentNameText;

            if (yourScorePanel != null)
            {
                yourScorePanel.gameObject.SetActive(false);
            }
        }
        else
        {
            SoundManager.Instance.PlaySound(nameInputController.submitSound);
            StartCoroutine(DelayRegistrationComplete());
            nameInputController.gameObject.SetActive(false);
            currentNameText.gameObject.SetActive(false);

            if (yourScoreText != null && yourScorePanel != null)
            {
                yourScorePanel.gameObject.SetActive(true);
                yourScoreText.text = $"YOUR SCORE WAS NOT RANKED\n\nSCORE: {finalScore,8} ({finalProgress})";
            }
        }

        DisplayRanking();
    }

    private void DisplayRanking()
    {
        var scores = RankingManager.Instance.rankingData.scores;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("== Score Ranking ==");

        for (int i = 0; i < 10; i++)
        {
            if (i < scores.Count)
            {
                sb.AppendLine($"{(i + 1),2}. {scores[i].playerName,-10} {scores[i].dateTime} {scores[i].score,8} ({scores[i].progress})");
            }
            else
            {
                sb.AppendLine($"{(i + 1),2}. ---");
            }
        }

        rankingListText.text = sb.ToString();
    }

    public void RegisterScore(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "NoName";
        }

        ScoreEntry newEntry = new ScoreEntry
        {
            playerName = playerName,
            score = finalScore,
            dateTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm"),
            progress = finalProgress
        };

        RankingManager.Instance.AddScore(newEntry);

        nameInputController.gameObject.SetActive(false);
        DisplayRanking();

        StartCoroutine(DelayRegistrationComplete());

        if (yourScoreText != null && yourScorePanel != null)
        {
            yourScorePanel.gameObject.SetActive(true);
            yourScoreText.text = $"YOUR SCORE WAS RANKED\n\nSCORE: {finalScore,8} ({finalProgress})";
        }

        if (currentNameText != null)
        {
            currentNameText.gameObject.SetActive(false);
        }
    }
    
    private IEnumerator DelayRegistrationComplete()
    {
        yield return new WaitForSecondsRealtime(1f);
        isRegistrationComplete = true;
    }

    public void ShowRankingViewOnly()
    {
        gameObject.SetActive(true);

        if (yourScorePanel != null)
        {
            yourScorePanel.gameObject.SetActive(false);
        }
        if (nameInputController != null)
        {
            nameInputController.gameObject.SetActive(false);
        }
        if (currentNameText != null)
        {
            currentNameText.gameObject.SetActive(false);
        }

        DisplayRanking();

        isRegistrationComplete = true;
    }
}
