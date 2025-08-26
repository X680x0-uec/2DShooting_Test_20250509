using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    
    // 出現位置の範囲（例: x座標を -10 ～ 10 にする）
    public float minY = -3f;
    public float maxY = 3f;
    public float fixedX = 9f;
    public float fixedZ = 0f;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(fixedX, randomY, fixedZ);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
