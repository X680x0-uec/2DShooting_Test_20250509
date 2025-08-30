using UnityEngine;

public class SingleChargeEnemy : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 direction;

    void Start()
    {
        // プレイヤーの位置を取得（出現時のみ）
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            direction = (playerPosition - transform.position).normalized;

            // 任意：向きをプレイヤー方向に向ける（2D回転）
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            Debug.LogError("Playerタグが設定されたプレイヤーが見つかりません。");
            direction = Vector3.right; // 予備：右方向に進む
        }
    }

    void Update()
    {
        // 記録済みの方向に直進
        transform.position += direction * speed * Time.deltaTime;
    }
}