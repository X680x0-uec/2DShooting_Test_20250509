using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    public float damage = 1f;
    public float damageInterval = 0.1f;
    public float appearDuration = 0.1f;
    public float disappearDuration = 0.15f;
    public GameObject debrisSpawnerPrefab;
    private PlayerController playerController;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Vector3 originalScale;

    // このレーザーに触れている敵を管理するリスト
    private System.Collections.Generic.List<ZakoHP> zakosInRange = new System.Collections.Generic.List<ZakoHP>();
    private System.Collections.Generic.List<BossHP> bossesInRange = new System.Collections.Generic.List<BossHP>();
    private float nextDamageTime;

    private IEnumerator Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        damage *= playerController.GetAttackMultiplier();

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalScale = transform.localScale;

        boxCollider.enabled = false;

        yield return StartCoroutine(AppearAnimation());

        boxCollider.enabled = true;
    }

    private IEnumerator AppearAnimation()
    {
        float timer = 0f;
        while (timer < appearDuration)
        {
            float progress = timer / appearDuration;

            float currentYScale = Mathf.Lerp(0, originalScale.y, progress);
            transform.localScale = new Vector3(originalScale.x, currentYScale, originalScale.z);

            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0, 0.8f, progress);
            spriteRenderer.color = color;

            timer += Time.deltaTime;
            yield return null; //1フレーム待機
        }
        transform.localScale = originalScale;
        Color finalColor = spriteRenderer.color;
        finalColor.a = 0.8f;
        spriteRenderer.color = finalColor;
    }

    public void StartDisappearAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(DisappearAnimation());
    }

    private IEnumerator DisappearAnimation()
    {
        boxCollider.enabled = false;
        this.enabled = false;

        float timer = 0f;
        while (timer < disappearDuration)
        {
            float progress = timer / disappearDuration;
            float currentYScale = Mathf.Lerp(originalScale.y, 0, progress);
            transform.localScale = new Vector3(originalScale.x, currentYScale, originalScale.z);
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0.8f, 0, progress);
            spriteRenderer.color = color;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (zakosInRange.Count >= 1 && Time.time >= nextDamageTime)
        {
            for (int i = zakosInRange.Count - 1; i >= 0; i--)
            {
                var enemy = zakosInRange[i];
                if (enemy == null)
                {
                    zakosInRange.RemoveAt(i);
                }
                else
                {
                    enemy.TakeDamage(damage);
                }
            }
            nextDamageTime = Time.time + damageInterval;
        }
        else if (bossesInRange.Count >= 1 && Time.time >= nextDamageTime)
        {
            for (int i = bossesInRange.Count - 1; i >= 0; i--)
            {
                var enemy = bossesInRange[i];
                if (enemy == null)
                {
                    bossesInRange.RemoveAt(i);
                }
                else
                {
                    enemy.TakeDamage(damage);
                }
            }
            nextDamageTime = Time.time + damageInterval;
        }
    }

    // 敵または敵弾がレーザーの当たり判定に入った時
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Instantiate(debrisSpawnerPrefab, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Zako"))
        {
            ZakoHP zakoHP = other.GetComponent<ZakoHP>();
            if (zakoHP != null && !zakosInRange.Contains(zakoHP))
            {
                zakosInRange.Add(zakoHP); // リストに追加
            }
        }
        else if (other.CompareTag("Boss"))
        {
            BossHP bossHP = other.GetComponent<BossHP>();
            if (bossHP != null && !bossesInRange.Contains(bossHP))
            {
                bossesInRange.Add(bossHP); // リストに追加
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
                zakosInRange.Remove(zakoHP); // リストから削除
            }
        }
        else if (other.CompareTag("Boss"))
        {
            BossHP bossHP = other.GetComponent<BossHP>();
            if (bossHP != null)
            {
                bossesInRange.Remove(bossHP); // リストから削除
            }
        }
    }
}
