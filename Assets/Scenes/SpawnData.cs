using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;   // 出現させる敵
    public Vector2 spawnPosition;    // 出現位置
    public float spawnTime;          // スポーンまでの時間
}
