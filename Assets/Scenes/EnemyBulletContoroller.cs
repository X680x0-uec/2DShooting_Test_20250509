using UnityEngine;

public class EnemyBulletContoroller : MonoBehaviour

{
    public float speed = 5f;
    public float angle = 0f;
    
    void Update()
    {
        
        // 弾を移動
        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        // 画面外チェック
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if(screenPos.x < 0f || screenPos.x > 1f || screenPos.y < 0f || screenPos.y > 1f)
        {
            Destroy(gameObject);
        }
    }
}

