using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum SpecialSkillType
{
    Empty,
    Laser,
    Vanish,
    Bomb,
    TimeStop,
    CounterAttack
}

public class PlayerController : MonoBehaviour
{
    [Header("ビジュアル")]
    public float screenEdgeDistance = 0.3f;
    public CircleCollider2D hitboxCollider;
    public float blinkInterval = 0.1f; //点滅間隔
    private SpriteRenderer playerSpriteRenderer;
    private Vector2 screenBounds; //画面端の位置

    [Header("自機弾関連")]
    public GameObject bulletPrefab;
    public Transform[] firePoints; //発射位置
    public float fireRate = 0.15f;
    private float nextFireTime = 0.0f;

    [Header("ゲーム用パラメータ")]
    public float moveSpeed = 10.0f;
    public float slowMoveSpeed = 3.0f;
    public float attackMultiplier = 1.0f;
    public int life = 5;
    public SpecialSkillType currentSkill = SpecialSkillType.Empty; //セット中のスキル
    public int supecialSkillUsesLeft = 0;
    private bool isUsingSpecialSkill = false;
    private bool isInvincible = false;
    public float invincibilityDuration = 2.0f; //被弾時無敵の長さ
    private bool isControllLocked = false;
    public float controlLockDuration = 0.2f; //被弾時操作不能時間の長さ

    [Header("特殊スキル用")]
    public GameObject laserPrefab;
    public float laserDuration = 4.0f;
    public float laserDurationMultiplier = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //端の座標を取得
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Transform visualChild = transform.Find("Visual");
        if (visualChild != null)
        {
            playerSpriteRenderer = visualChild.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogWarning("Playerの子オブジェクトに'Visual'が見つかりません。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isControllLocked)
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

            newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x + screenEdgeDistance, screenBounds.x - screenEdgeDistance);
            newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y + screenEdgeDistance, screenBounds.y - screenEdgeDistance);

            transform.position = newPosition;

            //弾の発射
            if (Input.GetKey(KeyCode.Z) && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }

            //スキル発動
            if (Input.GetKey(KeyCode.X) && supecialSkillUsesLeft > 0 && !isUsingSpecialSkill && currentSkill != SpecialSkillType.Empty)
            {
                UseSpecialSkill();
            }
        }
    }

    void Shoot()
    {
        foreach (Transform point in firePoints)
        {
            Instantiate(bulletPrefab, point.position, point.rotation);
        }
    }

    public void GetSkill(string category, int id, int level)
    {
        switch (category)
        {
            case "special":
                switch (id)
                {
                    case 0:
                        switch (level)
                        {
                            case 0:
                                currentSkill = SpecialSkillType.Laser;
                                break;
                            case 1:
                                laserDurationMultiplier = 1.5f;
                                break;
                        }
                        break;
                }
                break;
            case "attack":
                switch (level)
                {
                    case 1:
                        attackMultiplier = 1.2f;
                        break;
                    case 2:
                        attackMultiplier = 1.3f;
                        break;
                    case 3:
                        attackMultiplier = 1.5f;
                        break;
                }
                break;
        }
    }

    void UseSpecialSkill()
    {
        isUsingSpecialSkill = true;
        supecialSkillUsesLeft--;

        switch (currentSkill)
        {
            case SpecialSkillType.Laser:
                StartCoroutine(LaserCoroutine());
                break;
        }
    }

    private IEnumerator LaserCoroutine()
    {
        Vector3 laserPosition = transform.position + new Vector3(13, 0, 0);
        GameObject laserObject = Instantiate(laserPrefab, laserPosition, Quaternion.identity);

        laserObject.transform.SetParent(this.transform); //レーザーをプレイヤーの子オブジェクト化

        yield return new WaitForSeconds(laserDuration * laserDurationMultiplier); //持続時間待機

        Destroy(laserObject);
        isUsingSpecialSkill = false;
    }

    public void StartBlinkEffect()
    {
        if (!isInvincible && playerSpriteRenderer != null) //無敵中でなければ開始
        {
            StartCoroutine(BlinkEffectCoroutine());
        }
    }

    private IEnumerator BlinkEffectCoroutine()
    {
        isInvincible = true;
        isControllLocked = true;
        // 当たり判定のコライダーを無効化
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }
        yield return new WaitForSeconds(controlLockDuration); //被弾時に一瞬動けなくする
        isControllLocked = false;
        float invincibleEndTime = Time.time + invincibilityDuration - controlLockDuration;

        //無敵時間待機
        while (Time.time < invincibleEndTime)
        {
            playerSpriteRenderer.enabled = !playerSpriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        playerSpriteRenderer.enabled = true;
        isInvincible = false;
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true;
        }
    }
}
