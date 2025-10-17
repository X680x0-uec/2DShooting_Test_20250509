using UnityEngine;
using System.Collections.Generic;

public class HomingBulletMovement : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 200f;
    public float searchRadius = 10f;
    public string enemyTag1 = "Zako";
    public string enemyTag2 = "Boss";

    private Rigidbody2D rb;
    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindClosestEnemy();
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = transform.up * speed;
            FindClosestEnemy();
            return;
        }
        Vector2 directionToTarget = (Vector2)target.position - rb.position;
        directionToTarget.Normalize();

        //外積計算
        float rotateAmount = Vector3.Cross(directionToTarget, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.linearVelocity = transform.up * speed;
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
            if (distance < closestDistance && distance <= searchRadius)
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
