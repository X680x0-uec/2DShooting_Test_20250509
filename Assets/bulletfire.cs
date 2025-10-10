using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // 左方向に移動（2Dなので x軸だけ動かす）
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // 画面外に出たら削除（メモリ節約）
        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
