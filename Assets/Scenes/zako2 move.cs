using UnityEngine;

public class Enemy2Movement : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        // 左方向に移動（ローカル座標ではなくワールド座標で）
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
    
}
