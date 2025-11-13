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
