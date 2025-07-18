using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //移動速度
    public float moveSpeed = 10.0f;
    public float slowMoveSpeed = 3.0f;
    public float hitboxRadius = 0.15f;
    public CircleCollider2D hitboxCollider;
    public GameObject hitboxVisual;

    //画面端
    private Vector2 screenBounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //端の座標を取得
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        if (hitboxCollider != null)
        {
            // コライダーの半径を設定
            hitboxCollider.radius = hitboxRadius;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //シフト低速移動
        float currentSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = slowMoveSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // 左右入力の取得
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        //位置計算
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        Vector3 newPosition = transform.position + (Vector3)(moveDirection * currentSpeed * Time.deltaTime);

        newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x, screenBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y, screenBounds.y);

        transform.position = newPosition;
    }
}
