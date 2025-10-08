using UnityEngine;

public class Enemy3Movement : MonoBehaviour
{
    public float speed = 2f;          // 左方向への移動速度
    public float amplitude = 1f;      // 上下の振れ幅（高さ）
    public float frequency = 2f;      // 上下の速さ（周波数）

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // X座標：時間に応じて左に移動
        float newX = transform.position.x - speed * Time.deltaTime;

        // Y座標：Sin波で上下に振れる（startPosition.yを中心に）
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;

        transform.position = new Vector3(newX, newY, startPosition.z);
    }

}
