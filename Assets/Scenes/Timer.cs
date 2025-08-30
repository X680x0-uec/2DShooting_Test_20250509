using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeLimit = 60f;               // 制限時間（秒）
    public TextMeshProUGUI timerText;           // タイマー表示用UI
    public GameObject bossPrefab;               // 出現させたいボスのプレハブ
    public Vector2 bossSpawnPosition = new Vector2(0, 5); // ボスの出現位置
    public EnemySpawner[] enemySpawners;        // 雑魚スポナー（任意）

    private float remainingTime;
    private bool isTimeUp = false;

    void Start()
    {
        remainingTime = timeLimit;
    }

    void Update()
    {
        if (isTimeUp) return;

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(remainingTime, 0);
        UpdateTimerUI();

        if (remainingTime <= 0f)
        {
            isTimeUp = true;
            OnTimeUp();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTimeUp()
    {
        Debug.Log("タイマーが0！ボス出現！");
        SpawnBoss();

        // 任意：雑魚の出現を止める
        foreach (var spawner in enemySpawners)
        {
            spawner.canSpawn = false;
        }

        // 任意：既に出現している雑魚を全て消す
        ClearAllEnemies();

        if (timerText != null)
    {
        Destroy(timerText.gameObject);
    }
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
    }

    void ClearAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("damage");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}