using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyWaveManager : MonoBehaviour
{
    private GameDataJson gameData;       // 現在のステージデータ
    private int currentStage = 1;        // 1からスタート
    private bool bossStarted = false;

    private Coroutine enemyCoroutine;
    private Coroutine bossCoroutine;

    [Header("ステージ切替演出")]
    public float stageClearDelay = 2.0f;

    void Start()
    {
        LoadAndStartStage(currentStage);
    }

    // ステージをロードして開始
    void LoadAndStartStage(int stageNumber)
    {
        gameData = StageLoader.LoadGameData(stageNumber);

        if (gameData == null)
        {
            Debug.LogError($"ステージ{stageNumber} のデータが存在しません。全ステージクリア扱いにします。");
            return;
        }

        StartStage(gameData.stages[0]);
    }

    void StartStage(StageJson stage)
    {
        if (enemyCoroutine != null) StopCoroutine(enemyCoroutine);
        if (bossCoroutine != null) StopCoroutine(bossCoroutine);

        bossStarted = false;
        enemyCoroutine = StartCoroutine(SpawnEnemies(stage.enemies));
        bossCoroutine = StartCoroutine(StartBossTimer(stage.boss.spawnTime, stage.boss));
    }

    IEnumerator SpawnEnemies(EnemySpawnJson[] spawnList)
    {
        float timer = 0f;
        int nextIndex = 0;

        while (nextIndex < spawnList.Length)
        {
            timer += Time.deltaTime;
            EnemySpawnJson data = spawnList[nextIndex];

            if (timer >= data.spawnTime)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(data.prefabName);
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject enemy = Instantiate(handle.Result, data.spawnPosition, Quaternion.identity);
                    enemy.tag = "Zako";
                }
                else
                {
                    Debug.LogWarning("Addressable Prefab not found: " + data.prefabName);
                }

                nextIndex++;
            }

            yield return null;
        }

        Debug.Log($"ステージ{currentStage}の雑魚を全て出しました。");
    }

    IEnumerator StartBossTimer(float waitTime, BossJson bossData)
    {
        yield return new WaitForSeconds(waitTime);

        if (!bossStarted)
        {
            bossStarted = true;

            ClearEnemiesAndBullets();
            StartCoroutine(SpawnBossAddressable(bossData));
        }
    }

    IEnumerator SpawnBossAddressable(BossJson bossData)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(bossData.prefabName);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject boss = Instantiate(handle.Result, bossData.spawnPosition, Quaternion.identity);
            boss.tag = "Boss";
            InformationUIController.Instance.ShowBossHP(true);
            Debug.Log($"ステージ{currentStage} ボス戦開始！");
        }
        else
        {
            Debug.LogWarning("Addressable Boss Prefab not found: " + bossData.prefabName);
        }
    }

    void ClearEnemiesAndBullets()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Zako");
        foreach (GameObject enemy in enemies) Destroy(enemy);

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject bullet in bullets) Destroy(bullet);

        Debug.Log("雑魚と弾を消去しました。");
    }

    public void OnBossDefeated()
    {
        bossStarted = false;

        if (bossCoroutine != null)
        {
            StopCoroutine(bossCoroutine);
            bossCoroutine = null;
        }

        InformationUIController.Instance.ShowBossHP(false);
        StartCoroutine(HandleStageClear());
    }

    private IEnumerator HandleStageClear()
    {
        Debug.Log($"ステージ{currentStage} クリア演出開始");
        yield return new WaitForSeconds(stageClearDelay);

        currentStage++;
        if (currentStage <= 3) // 3ステージまで
        {
            Debug.Log($"次のステージへ: {currentStage}");
            LoadAndStartStage(currentStage);
        }
        else
        {
            Debug.Log("全ステージクリア！");
            // ここでゲームクリア画面に遷移する処理を追加可能
        }
    }
}
