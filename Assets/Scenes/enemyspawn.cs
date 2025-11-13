using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool canSpawn = true;
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;

    public float minY = -3f;
    public float maxY = 3f;
    public float fixedX = 9f;
    public float fixedZ = 0f;

    private float timer;

    void Update()
    {
        if (!canSpawn) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            float randomY = Random.Range(minY, maxY);
            Vector3 spawnPosition = new Vector3(fixedX, randomY, fixedZ);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            timer = 0f;
        }
    }
}
