using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damage = 1f;
    public float damageInterval = 0.1f;

    // このレーザーに触れている敵を管理するリスト
    private System.Collections.Generic.List<ZakoHP> enemiesInRange = new System.Collections.Generic.List<ZakoHP>();
    private float nextDamageTime;

    // Update is called once per frame
    void Update()
    {
        if (enemiesInRange.Count >= 1 && Time.time >= nextDamageTime)
        {
            foreach (var enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            nextDamageTime = Time.time + damageInterval;
        }
    }

    // 敵がレーザーの当たり判定に入った時
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zako"))
        {
            zakoHP = other.GetComponent<ZakoHP>();
            if (zakoHP != null && !enemiesInRange.Contains(zakoHP))
            {
                enemiesInRange.Add(zakoHP); // リストに追加
            }
        }
    }

    // 敵がレーザーの当たり判定から出た時
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zako"))
        {
            ZakoHP zakoHP = other.GetComponent<ZakoHP>();
            if (zakoHP != null)
            {
                enemiesInRange.Remove(zakoHP); // リストから削除
            }
        }
    }
}
