using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damage = 1f;
    public float damageInterval = 0.1f;

    // このレーザーに触れている敵を管理するリスト
    //private System.Collections.Generic.List<EnemyHealth> enemiesInRange = new System.Collections.Generic.List<EnemyHealth>();
    private float nextDamageTime;

    // Update is called once per frame
    /*
    void Update()
    {
        if (Time.time >= nextDamageTime)
        {
            foreach (var enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    //敵にダメージを与える処理
                }
            }
        }
    }

    // 敵がレーザーの当たり判定に入った時
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null && !enemiesInRange.Contains(enemyHealth))
            {
                enemiesInRange.Add(enemyHealth); // リストに追加
            }
        }
    }

    // 敵がレーザーの当たり判定から出た時
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemiesInRange.Remove(enemyHealth); // リストから削除
            }
        }
    }
    */
}
