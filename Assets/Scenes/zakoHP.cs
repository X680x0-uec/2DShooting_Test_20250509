using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZakoHP : MonoBehaviour
{
    [Header("HP設定")]
    public float maxHP = 100f;
    private float currentHP;

    [Header("スコア設定")]
    public int pointValue = 100;
    [SerializeField] private GameObject scorePopupPrefab;

    [Header("Canvas")]
    [SerializeField] private Canvas uiCanvas;

    [Header("敵破壊条件")]
    public float destroyX = -10f;

    [Header("スキルシステム")]
    public SkillSystem skillSystem;

    void Start()
    {
        currentHP = maxHP;

        // SkillSystem が未設定なら自動取得（非アクティブも含む）
        if (skillSystem == null)
        {
            skillSystem = Object.FindFirstObjectByType<SkillSystem>(FindObjectsInactive.Include);
            if (skillSystem == null)
                Debug.LogError("SkillSystem が見つかりません");
        }

        // Canvas を保証
        EnsureCanvas();
    }

    void Update()
    {
        // 敵が画面外に行ったら破壊
        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }

    // ダメージを受ける
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("敵の現在HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // 敵死亡時
    void Die()
    {
        // 先にスコアポップアップ表示
        ShowScorePopup();

        // スキルポイント付与
        if (skillSystem != null)
            skillSystem.TakeSkillPoint(pointValue);

        Destroy(gameObject);
    }

    // スコアポップアップ表示
    private void ShowScorePopup()
    {
        if (scorePopupPrefab == null || uiCanvas == null)
            return;

        // 敵のワールド座標 → スクリーン座標
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // Canvas 上に生成
        GameObject popup = Instantiate(scorePopupPrefab, uiCanvas.transform);

        // 位置設定
        RectTransform rect = popup.GetComponent<RectTransform>();
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiCanvas.transform as RectTransform,
            screenPos,
            uiCanvas.worldCamera,
            out localPos
        );
        rect.localPosition = localPos;

        // テキスト設定
        ScorePopupUI popupUI = popup.GetComponent<ScorePopupUI>();
        if (popupUI != null)
        {
            popupUI.SetText($"+{pointValue}P");
        }

        // 任意：自動で消える
        Destroy(popup, 1.5f);
    }

    // Canvas を自動生成する
    private void EnsureCanvas()
    {
        if (uiCanvas == null)
        {
            // 非推奨警告を回避
            uiCanvas = Object.FindFirstObjectByType<Canvas>();
            if (uiCanvas == null)
            {
                GameObject canvasGO = new GameObject("ScorePopupCanvas");
                uiCanvas = canvasGO.AddComponent<Canvas>();
                canvasGO.AddComponent<CanvasScaler>();
                canvasGO.AddComponent<GraphicRaycaster>();
                uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                Debug.Log("Canvas がなかったので自動生成しました");
            }
        }
    }
}
