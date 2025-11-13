using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class Laser : MonoBehaviour
{
    public float damage = 1f;
    public float damageInterval = 0.1f;
    
    public GameObject debrisSpawnerPrefab;
    private PlayerController playerController;

    private Animator anim;
    private BoxCollider2D boxCollider;

    // このレーザーに触れている敵を管理するリスト
    private System.Collections.Generic.List<ZakoHP> zakosInRange = new System.Collections.Generic.List<ZakoHP>();
    private System.Collections.Generic.List<BossHP> bossesInRange = new System.Collections.Generic.List<BossHP>();
    private float nextDamageTime;

    void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        boxCollider.enabled = false;
        this.enabled = false;
    }

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        damage *= playerController.GetAttackMultiplier();
    }

    public void ActivateDamage() //アニメーションから呼ぶ
    {
        boxCollider.enabled = true;
        this.enabled = true;
    }

    public void TriggerEndAnimation() //playerから呼ぶ
    {
        this.enabled = false;
        boxCollider.enabled = false;

        anim.SetTrigger("EndLaser");
    }

    public void DestroySelf() //アニメーションから呼ぶ
    {
        Destroy(gameObject);
    }

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
