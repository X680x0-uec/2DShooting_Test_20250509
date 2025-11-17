using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CurvingBullet : MonoBehaviour
{
    [Header("カーブ設定")]
    [Tooltip("1秒間に回転する角度（度数法）")]
    public float curveStrength = 180f; 

    private Rigidbody2D rb;
    private int curveDirection = 0; // 0=直進, 1=反時計回り, -1=時計回り

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(int direction)
    {
        curveDirection = direction;
    }

    void FixedUpdate()
    {
        if (curveDirection == 0)
        {
            return;
        }

        Vector2 currentVelocity = rb.linearVelocity;

        float rotationAmount = curveStrength * Time.fixedDeltaTime * curveDirection;

        Vector2 newVelocity = Quaternion.Euler(0, 0, rotationAmount) * currentVelocity;

        rb.linearVelocity = newVelocity;
    }
}