using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    public SkillNode up;
    public SkillNode down;
    public SkillNode left;
    public SkillNode right;

    private Button button;
    private Image image;

    [Header("スキル情報")]
    public SkillParam skillParam;

    [Header("Scroll関連")]
    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform viewport;
    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void Select()
    {
        image.color = Color.blue; // ハイライト

        if (skillParam != null)
            skillParam.SetText();

        ScrollIntoView();
    }

    public void Deselect()
    {
        image.color = Color.white;

        if (skillParam != null)
            skillParam.ResetText();
    }

    public SkillNode GetNextNode(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return up;
        if (direction == Vector2Int.down) return down;
        if (direction == Vector2Int.left) return left;
        if (direction == Vector2Int.right) return right;
        return null;
    }

    public void Press()
    {
        if (button != null)
    {
        button.onClick.Invoke(); // ← これでUIクリックと同じ処理が走る
    }
        else if (skillParam != null)
            skillParam.OnClick();
    }

    private void ScrollIntoView()
    {
        if (scrollRect == null || scrollRect.content == null || scrollRect.viewport == null)
            return;

        Canvas.ForceUpdateCanvases();

        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;
        RectTransform buttonRect = transform as RectTransform;

        float contentHeight = content.rect.height;
        float contentWidth = content.rect.width;
        float viewportHeight = viewport.rect.height;
        float viewportWidth = viewport.rect.width;

        // ボタンの Content 内ローカル座標
        Vector3 localPos = content.InverseTransformPoint(buttonRect.position);

        // ボタンの上下端
        float buttonTop = -localPos.y + buttonRect.rect.height * (1 - buttonRect.pivot.y);
        float buttonBottom = -localPos.y - buttonRect.rect.height * buttonRect.pivot.y;

        // ボタンの左右端
        float buttonLeft = localPos.x - buttonRect.rect.width * buttonRect.pivot.x;
        float buttonRight = localPos.x + buttonRect.rect.width * (1 - buttonRect.pivot.x);

        Vector2 scrollPos = content.anchoredPosition;

        // 縦スクロール
        if (buttonBottom < scrollPos.y)
            scrollPos.y = Mathf.Clamp(buttonBottom-20, 0, contentHeight - viewportHeight);
        else if (buttonTop > scrollPos.y + viewportHeight)
            scrollPos.y = Mathf.Clamp(buttonTop - viewportHeight+20, 0, contentHeight - viewportHeight);

        // 横スクロール
        if (buttonRight > -scrollPos.x)
            scrollPos.x = Mathf.Clamp(-buttonRight-10, 0, contentWidth - viewportWidth);
        else if (buttonLeft < -scrollPos.x - viewportWidth)
            scrollPos.x = Mathf.Clamp(-buttonLeft - viewportWidth+10, 0, contentWidth - viewportWidth);

        // 更新
        content.anchoredPosition = scrollPos;
    }
}

