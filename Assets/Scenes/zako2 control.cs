using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("ハンマー設定")]
    [SerializeField] private GameObject hammer;      // ハンマープレハブ
    [SerializeField] private float throwIntervalSec = 1f;  // 投げる間隔
    [SerializeField] private float throwSpeed = 10f;       // 飛ぶ速度
    [SerializeField] private float hammerSize = 0.5f;      // ハンマーのサイズ
    [SerializeField] private float destroyDelaySec = 0.03f; // ハンマー消滅までの遅延

    private Transform player;

    void Start()
    {
        // タグ "Player" からプレイヤー取得
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("タグ 'Player' のオブジェクトが見つかりません。");
            return;
        }

        if (hammer == null)
        {
            Debug.LogError("ハンマープレハブが設定されていません。");
            return;
        }

        StartCoroutine(ThrowHammerCoroutine());
    }

    IEnumerator ThrowHammerCoroutine()
    {
        while (true)
        {
            // 投げる瞬間のプレイヤー位置
            Vector2 targetPos = player.position;
            Vector2 spawnPos = transform.position;

            // 方向ベクトル
            Vector2 direction = (targetPos - spawnPos).normalized;

            // ハンマー生成
            GameObject hammerInstance = Instantiate(hammer, transform.position, Quaternion.identity);
            hammerInstance.transform.localScale = new Vector3(hammerSize, hammerSize, 1);

            Rigidbody2D rb = hammerInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;                  // 重力無効
                rb.linearVelocity = direction * throwSpeed;  // 直線で飛ばす
            }
            else
            {
                Debug.LogError("ハンマーに Rigidbody2D コンポーネントがありません。");
            }

            HammerController hc = hammerInstance.GetComponent<HammerController>();
            if (hc != null)
            {
                hc.delaySec = destroyDelaySec;
            }

            yield return new WaitForSeconds(throwIntervalSec);
        }
    }
}
