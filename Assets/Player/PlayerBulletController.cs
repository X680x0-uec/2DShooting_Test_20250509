using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    // 弾の速度
    public float speed = 15f;
    public float baseDamage = 10f;
    // 弾が自動で消えるまでの時間
    private float lifeTime = 3f;
    private float finalDamage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;

        Destroy(gameObject, lifeTime);
    }

    public void Initialize(float playerAttackMultiplier)
    {
        finalDamage = baseDamage * playerAttackMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zako"))
        {
            ZakoHP zakoHP = other.gameObject.GetComponent<ZakoHP>();
            if (zakoHP != null)
            {
                zakoHP.TakeDamage(finalDamage);
            }
            else
            {
                Debug.LogWarning($"オブジェクト '{other.gameObject.name}' にはZakoHPスクリプトがありません。");
            }

            Destroy(gameObject);
        }
    }
}
