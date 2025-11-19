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
            new ScoreEntry { playerName = "PLAYER1", score = 500000, dateTime = "2025/11/19 00:00", progress = "SAMPLE" },
            new ScoreEntry { playerName = "PLAYER2", score = 400000, dateTime = "2025/11/19 00:00", progress = "SAMPLE" },
            new ScoreEntry { playerName = "PLAYER3", score = 300000, dateTime = "2025/11/19 00:00", progress = "SAMPLE" },
            new ScoreEntry { playerName = "PLAYER4", score = 200000, dateTime = "2025/11/19 00:00", progress = "SAMPLE" },
            new ScoreEntry { playerName = "PLAYER5", score = 100000, dateTime = "2025/11/19 00:00", progress = "SAMPLE" },
            new ScoreEntry { playerName = "PLAYER6", score = 50000, dateTime = "2025/11/19 00:00", progress = "SAMPLE" }
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
