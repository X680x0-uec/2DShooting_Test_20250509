using UnityEngine;

public class ParentBullet : MonoBehaviour
{
    [Header("設定")]
    public GameObject childBulletPrefab;
    public int childBulletCount = 4;
    public float childSpreadAngle = 120f;
    public float speed = 4f;
    public float decelerationRate = 2f;
    public float splitTime = 3f;
    public float destroyDelay = 0.5f;

    [Header("SE設定")]
    public AudioClip splitSE;    // ← 分裂時のサウンド

    private Rigidbody2D rb;
    private bool hasSplit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.left * speed; // 左向きに発射

        // splitTime 秒後に分裂
        Invoke(nameof(Split), splitTime);
    }

    void Update()
    {
        if (!hasSplit)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, decelerationRate * Time.deltaTime);
        }
    }

    void Split()
    {
        if (hasSplit) return;
        hasSplit = true;

        // ★ 分裂SEを再生
        if (splitSE != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(splitSE, 0.7f); // ← 音量0.7（適宜調整OK）
        }

        // 左方向（180度）を中心に扇状に発射
        float startAngle = 180f - (childSpreadAngle / 2f);
        float angleStep = (childBulletCount > 1) ? (childSpreadAngle / (childBulletCount - 1)) : 0f;

        for (int i = 0; i < childBulletCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            float rad = currentAngle * Mathf.Deg2Rad;

            // ← 左方向を中心とした扇形方向を計算
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject child = Instantiate(childBulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D childRb = child.GetComponent<Rigidbody2D>();
            if (childRb != null)
            {
                childRb.linearVelocity = direction.normalized * speed;
            }
        }

        Destroy(gameObject, destroyDelay);
    }
}
