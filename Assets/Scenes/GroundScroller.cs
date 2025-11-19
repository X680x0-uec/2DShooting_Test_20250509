using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float speed = 1f;

    float width;

    void Start()
    {
        // Sprite の実際の表示幅を取得
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 画像が左へ流れ切ったら右に戻す
        if (transform.position.x <= -width)
        {
            Vector3 pos = transform.position;
            pos.x += width * 2f; // A→B→A にループするので *2
            transform.position = pos;
        }
    }
}
