using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }
    public RankingData rankingData;

    private string savePath;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        savePath = Path.Combine(Application.persistentDataPath, "ranking.json");

        LoadRanking();
    }

    public void LoadRanking()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            rankingData = JsonUtility.FromJson<RankingData>(json);
        }
        else
        {
            rankingData = new RankingData();

            rankingData.scores = new List<ScoreEntry>
        {
            new ScoreEntry { playerName = "28", score = 3678729, dateTime = "2025/11/19 21:03", progress = "NO SP CLEAR"},
            new ScoreEntry { playerName = "+", score = 401359, dateTime = "2025/11/19 21:10", progress = "DIED IN STAGE3" },
            new ScoreEntry { playerName = "SAMPLE1", score = 300000, dateTime = "2025/11/19 00:00", progress = "SAMPLE" },
            new ScoreEntry { playerName = "SAMPLE2", score = 200000, dateTime = "2025/11/19 00:00", progress = "SAMPLE" },
            new ScoreEntry { playerName = "SAMPLE3", score = 100000, dateTime = "2025/11/19 00:00", progress = "SAMPLE" }
        };

            SaveRanking();
        }
    }

    public void AddScore(ScoreEntry newEntry)
    {
        rankingData.scores.Add(newEntry);

        rankingData.scores = rankingData.scores.OrderByDescending(entry => entry.score).ToList();

        if (rankingData.scores.Count > 10)
        {
            rankingData.scores = rankingData.scores.GetRange(0, 10);
        }

        SaveRanking();
    }

    private void SaveRanking()
    {
        string json = JsonUtility.ToJson(rankingData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"Data Saved in '{savePath}'");
    }

    public bool CheckIfRanked(int newScore)
    {
        if (rankingData.scores.Count < 10)
        {
            return true;
        }
        return newScore > rankingData.scores[9].score;
    }
}
