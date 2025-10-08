using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3.0f;
    private Transform target;

    void Start()
    {
        // タグを使ってプレイヤーを探す
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        if (target != null)
        {
            // プレイヤーの方向に移動
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}