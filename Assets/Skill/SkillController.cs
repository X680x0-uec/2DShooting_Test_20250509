using UnityEngine;

public class SkillController : MonoBehaviour
{
    public SkillNode startNode;
    private SkillNode currentNode;

    void Start()
    {
        currentNode = startNode;
        currentNode.Select();
    }

    void Update()
    {
        Vector2Int dir = Vector2Int.zero;

        // --- 矢印キー入力で移動 ---
        if (Input.GetKeyDown(KeyCode.UpArrow)) dir = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.DownArrow)) dir = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) dir = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) dir = Vector2Int.right;

        if (dir != Vector2Int.zero)
        {
            SkillNode next = currentNode.GetNextNode(dir);
            if (next != null)
            {
                currentNode.Deselect();
                currentNode = next;
                currentNode.Select();
            }
        }

        // --- Enterキーでボタンを押す ---
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            currentNode.Press();
        }
    }
}
