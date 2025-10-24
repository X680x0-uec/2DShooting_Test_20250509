using UnityEngine;
using TMPro;

public class ScorePopupUI : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 50f;      // 上に浮く速さ（ピクセル/秒）
    [SerializeField] private float lifetime = 1f;         // 表示時間
    [SerializeField] private float fadeDuration = 0.5f;   // フェード時間

    private TextMeshProUGUI text;
    private RectTransform rectTransform;
    private Color originalColor;
    private float timer = 0f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        originalColor = text.color;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // 上に移動
        rectTransform.anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;

        // フェードアウト
        if (timer > lifetime - fadeDuration)
        {
            float fade = 1 - ((timer - (lifetime - fadeDuration)) / fadeDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, fade);
        }

        // 一定時間後に削除
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string message)
    {
        text.text = message;
    }
}
