using UnityEngine;  
using System;

[System.Serializable]
public class GameDataJson
{
    public StageJson[] stages;
}

[System.Serializable]
public class StageJson
{
    public int stageId;
    public EnemySpawnJson[] enemies;
    public BossJson boss;
}

[System.Serializable]
public class EnemySpawnJson
{
    public string prefabName;
    public float spawnTime;
    public Vector2 spawnPosition;
}

[System.Serializable]
public class BossJson
{
    public string prefabName;
    public float spawnTime;
    public Vector2 spawnPosition;
}
