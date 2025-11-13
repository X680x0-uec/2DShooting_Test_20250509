using UnityEngine;

public class EnemyMoveDown : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 5f;         // 下方向へのスピード
    public float lifeTime = 3f;      // 何秒後に消えるか

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 下方向へ移動させる
        rb.linearVelocity = new Vector2(0, -speed);

        // lifeTime 秒後に敵を削除
        Destroy(gameObject, lifeTime);
    }
}
