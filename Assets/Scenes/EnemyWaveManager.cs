using UnityEngine;
using System.Collections;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("ステージ1")]
    public EnemySpawnData[] stage1Spawns;
    public GameObject stage1Boss;
    public float stage1BossTime = 30f;

    [Header("ステージ2")]
    public EnemySpawnData[] stage2Spawns;
    public GameObject stage2Boss;
    public float stage2BossTime = 30f;

    [Header("ステージ3")]
    public EnemySpawnData[] stage3Spawns;
    public GameObject stage3Boss;
    public float stage3BossTime = 30f;

    private int currentStage = 1;
    private bool bossStarted = false;

    void Start()
    {
        StartStage(currentStage);
    }

    void StartStage(int stageNumber)
    {
        switch (stageNumber)
        {
            case 1:
                StartCoroutine(SpawnEnemies(stage1Spawns));
                StartCoroutine(StartBossTimer(stage1BossTime, stage1Boss));
                break;
            case 2:
                StartCoroutine(SpawnEnemies(stage2Spawns));
                StartCoroutine(StartBossTimer(stage2BossTime, stage2Boss));
                break;
            case 3:
                StartCoroutine(SpawnEnemies(stage3Spawns));
                StartCoroutine(StartBossTimer(stage3BossTime, stage3Boss));
                break;
        }
    }

    IEnumerator SpawnEnemies(EnemySpawnData[] spawnList)
    {
        float timer = 0f;
        int nextIndex = 0;

        while (nextIndex < spawnList.Length)
        {
            timer += Time.deltaTime;
            EnemySpawnData data = spawnList[nextIndex];

            if (timer >= data.spawnTime)
            {
                Instantiate(data.enemyPrefab, data.spawnPosition, Quaternion.identity);
                nextIndex++;
            }

            yield return null;
        }

        Debug.Log("ステージ" + currentStage + "の雑魚を全て出しました。");
    }

    IEnumerator StartBossTimer(float waitTime, GameObject bossPrefab)
    {
        yield return new WaitForSeconds(waitTime);

        if (!bossStarted)
        {
            bossStarted = true;
            SpawnBoss(bossPrefab);
        }
    }

    void SpawnBoss(GameObject bossPrefab)
    {
        if (bossPrefab != null)
        {
            Instantiate(bossPrefab, new Vector2(8, 0), Quaternion.identity);
            InformationUIController.Instance.ShowBossHP(true);
            Debug.Log("ステージ" + currentStage + " ボス戦開始！");
        }
        else
        {
            Debug.LogWarning("ボスPrefabが設定されていません！");
        }
    }

    // ボス撃破時に呼ぶ
    public void OnBossDefeated()
    {
        bossStarted = false;
        currentStage++;

        if (currentStage <= 3)
        {
            StartStage(currentStage);
        }
        else
        {
            Debug.Log("全ステージクリア！");
        }
    }
}
