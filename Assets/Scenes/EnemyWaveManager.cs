using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyWaveManager : MonoBehaviour
{
    private GameDataJson gameData;
    private int currentStage = 1;
    private bool bossStarted = false;

    private Coroutine enemyCoroutine;
    private Coroutine bossCoroutine;

    [Header("ステージ切替演出")]
    public float stageClearDelay = 2.0f;

    [Header("BGM設定")]
    public AudioSource bgmSource;
    public AudioClip stageBGM;
    public float loopStartTime = 60f; // 1分地点からループ
    private bool loopStarted = false; // ループ開始済みか

    void Start()
    {
        PlayStageBGM(); // 最初のステージ開始時は最初から再生
        LoadAndStartStage(currentStage);
    }

    // ステージ開始用BGM再生
    void PlayStageBGM()
    {
        if (bgmSource == null || stageBGM == null) return;

        bgmSource.Stop();
        bgmSource.clip = stageBGM;
        bgmSource.time = 0f;      // ステージ開始は最初から
        bgmSource.loop = false;
        bgmSource.Play();

        loopStarted = false; // 新ステージなのでループを再設定
        StartCoroutine(StartLoopFromTime(loopStartTime));
    }

    // 曲終了後に1分地点から永遠ループ開始
    private IEnumerator StartLoopFromTime(float startTime)
    {
        if (loopStarted) yield break; // すでにループ開始済みなら何もしない
        loopStarted = true;

        // 曲の終了まで待機
        yield return new WaitForSeconds(stageBGM.length - startTime);

        bgmSource.Stop();
        bgmSource.time = startTime;
        bgmSource.loop = true; // 永遠ループ
        bgmSource.Play();
    }

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
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Zako")) Destroy(enemy);
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("EnemyBullet")) Destroy(bullet);

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
        if (currentStage <= 3)
        {
            Debug.Log($"次のステージへ: {currentStage}");
            PlayStageBGM(); // 新ステージでは最初から再生
            LoadAndStartStage(currentStage);
        }
        else
        {
            Debug.Log("全ステージクリア！");
            // ゲームクリア画面へ遷移する処理を追加可能
        }
    }
}
