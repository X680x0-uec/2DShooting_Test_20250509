using System.IO;
using UnityEngine;

public class StageLoader
{
    // ← ここで stageNumber を引数にしているか確認
    public static GameDataJson LoadGameData(int stageNumber)
    {
        string fileName = $"stage{stageNumber}.json";       // stageNumber を使う
        string path = Path.Combine(Application.streamingAssetsPath, "Stages", fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"ステージデータが見つかりません: {fileName}");
            return null;
        }

        string jsonText = File.ReadAllText(path);
        GameDataJson gameData = JsonUtility.FromJson<GameDataJson>(jsonText);

        if (gameData == null || gameData.stages.Length == 0)
        {
            Debug.LogWarning($"ステージデータが不正です: {fileName}");
            return null;
        }

        Debug.Log($"ステージ{stageNumber} をロードしました: {fileName}");
        return gameData;
    }
}
