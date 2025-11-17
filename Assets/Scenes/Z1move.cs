using UnityEngine;

public class EnemyZMovementLeft : MonoBehaviour
{
    public float speed = 4f;           // 横方向の基本速度（左向き）
    public float zigzagAngle = 30f;    // Z 字の角度
    public float phaseTime = 0.5f;     // 方向切替時間

    private float timer = 0f;
    private int phase = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= phaseTime)
        {
            timer = 0f;
            phase++;
            if (phase > 2) phase = 0;  // 0→1→2→0…
        }

        Vector2 dir;

        switch (phase)
        {
            case 0: // ← 左（基本）
                dir = Vector2.left;
                break;

            case 1: // ↙ 左下（Z 字の斜め）
                dir = Quaternion.Euler(0, 0, zigzagAngle) * Vector2.left;
                break;

            case 2: // ← 左に戻る
                dir = Vector2.left;
                break;

            default:
                dir = Vector2.left;
                break;
        }

        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }
}
