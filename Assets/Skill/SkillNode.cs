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

    [Header("選択中の枠")]
    public Image highlightFrame; // 枠用のImage（Inspectorで設定）

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void Select()
    {
        image.color = Color.blue; // ハイライト
    }

    public void Deselect()
    {
        image.color = Color.white;
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
            button.onClick.Invoke(); // ボタンのクリックイベントを呼び出す
            Debug.Log($"{gameObject.name} のスキルが選択されました");
        }
    }
}
