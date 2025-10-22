using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomingBulletMovement : MonoBehaviour
{
    public float speed = 10f;
    //public float rotateSpeed = 200f;
    //public float trackingAngle = 90f; //実際の視野角の半分の数値を割り当て
    public string enemyTag1 = "Zako";
    public string enemyTag2 = "Boss";
    public float trackingDuration = 4f;

    private Rigidbody2D rb;
    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindClosestEnemy();
        if (target != null)
        {
            StartCoroutine(LoseTarget(trackingDuration));
        }
    }

    private IEnumerator LoseTarget(float duration)
    {
        yield return new WaitForSeconds(duration);
        target = null;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = transform.right * speed;
            return;
        }

        Vector2 directionToTarget = (Vector2)target.position - rb.position;

        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        rb.rotation = angle;
        
        /* 緩やかに曲がる追尾弾の場合(失敗作)
        float angleToTarget = Vector2.Angle(transform.right, directionToTarget);
        if (angleToTarget > trackingAngle)
        {
            target = null;
            Debug.Log("targetを見失いました");
            return;
        }

        directionToTarget.Normalize();

        //外積計算
        float rotateAmount = Vector3.Cross(directionToTarget, transform.right).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        */

        rb.linearVelocity = transform.right * speed;
    }

    void FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        GameObject[] enemies1 = GameObject.FindGameObjectsWithTag(enemyTag1);
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag(enemyTag2);
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(enemies1);
        enemies.AddRange(enemies2);

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
        }
    }
}
