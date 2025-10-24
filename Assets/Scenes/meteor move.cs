using UnityEngine;

public class Meteormove : MonoBehaviour
{
    public float speed = 5f;
    private void Update()
    {
        // 左方向に動かす（時間経過に依存しないようにTime.deltaTimeを使用）
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
