using UnityEngine;

public class FloatingBoss2D : MonoBehaviour
{
    public float floatSpeed = 1f;       // 浮くスピード（周期）
    public float floatHeight = 0.5f;    // 浮く高さ（上下の範囲）

    private Vector3 startPos;

    void Start()
    {
        // 最初の位置を記憶しておく
        startPos = transform.position;
    }

    void Update()
    {
        // Mathf.Sin を使ってY座標を上下に動かす（時間によって揺れる）
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z); // Zは使わないがそのまま
    }
}
