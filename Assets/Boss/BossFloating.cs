using UnityEngine;

public class BossFloating : MonoBehaviour
{
    public float amplitude = 1f;   // 揺れ幅（上下移動の大きさ）
    public float frequency = 1f;   // 周期（揺れる速さ）

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // 初期位置を記録
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
