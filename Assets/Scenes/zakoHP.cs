using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZakoHP : MonoBehaviour
{
    public SkillSystem skillSystem;
    public float maxHP = 100;
    private float currentHP;
    public int pointValue = 100;
    [SerializeField] private GameObject scorePopupPrefab;
    [SerializeField] private Canvas uiCanvas;
    void Start()
    {
        currentHP = maxHP;

        if (skillSystem == null)
        {
            skillSystem = FindFirstObjectByType<SkillSystem>(FindObjectsInactive.Include); // 非アクティブも対象にする


            if (skillSystem == null)
            {
                Debug.LogError("SkillSystem が見つかりません");
            }
        }

        uiCanvas = FindFirstObjectByType<Canvas>();
        
    }

    // ダメージを受ける関数
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("敵の現在HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // 敵が死ぬときの処理
    void Die()
    {
        skillSystem.TakeSkillPoint(pointValue);
        Destroy(gameObject);
        ShowScorePopup();
    }
    
    private void ShowScorePopup()
    {
        if (scorePopupPrefab != null && uiCanvas != null)
        {
            // 敵のワールド座標をスクリーン座標に変換
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            // Canvas上にインスタンス生成
            GameObject popup = Instantiate(scorePopupPrefab, uiCanvas.transform);

            // 表示位置を設定（anchoredPositionにスクリーン座標を使う）
            RectTransform rect = popup.GetComponent<RectTransform>();
            rect.position = screenPos;

            // テキストを設定
            popup.GetComponent<ScorePopupUI>().SetText($"+{pointValue}P");
        }
    }


        
}

