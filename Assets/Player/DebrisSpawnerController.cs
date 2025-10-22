using UnityEngine;

public class DebrisSpawnerController : MonoBehaviour
{
    public GameObject debrisPrefab;
    public int debrisCount = 3;
    public float debrisSpawnRadius = 0.3f;

    void Start()
    {
        if (debrisPrefab != null)
        {
            for (int i = 0; i < debrisCount; i++)
            {
                Vector3 randomOffset = (Vector3)Random.insideUnitCircle * debrisSpawnRadius;
                Vector3 spawnPosition = transform.position + randomOffset;
                Instantiate(debrisPrefab, spawnPosition, Quaternion.identity);
            }
            
        }
        Destroy(gameObject);
    }
}
