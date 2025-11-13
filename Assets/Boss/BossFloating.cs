using UnityEngine;

public class BossFloating : MonoBehaviour
{
    public float amplitude = 1f;   // 揺れ幅（上下移動の大きさ）
    public float frequency = 1f;   // 周期（揺れる速さ）

    private Vector3 startPos;
    private bool isPaused = false; // ← 無敵中に止めるフラグ

    void Start()
    {
        startPos = transform.position; // 初期位置を記録
    }

    void Update()
    {
        if (isPaused) return; // 無敵中は処理をスキップ

        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    // 無敵状態中に呼ぶメソッド
    public void SetPaused(bool pause)
    {
        isPaused = pause;
    }
}
