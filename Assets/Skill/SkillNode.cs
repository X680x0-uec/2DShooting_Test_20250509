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
        if (skillParam != null)
            skillParam.OnClick();
    }
}
