using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        // 左（-X方向）に進む
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }
}