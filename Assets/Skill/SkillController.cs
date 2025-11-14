using UnityEngine;

public class SkillController : MonoBehaviour
{
    public AudioClip moveSound;
    public SkillNode startNode;
    private SkillNode currentNode;

    private bool isHorizontalAxisInUse = false;
    private bool isVerticalAxisInUse = false;

    void Start()
    {
        currentNode = startNode;
        currentNode.Select();
    }

    void Update()
    {
        Vector2Int dir = Vector2Int.zero;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (!isHorizontalAxisInUse && x != 0)
        {
            if (x > 0.5f)
            {
                dir = Vector2Int.right;
            }
            else if (x < -0.5f)
            {
                dir = Vector2Int.left;
            }
            SoundManager.Instance.PlaySound(moveSound);
            isHorizontalAxisInUse = true;
        }
        else if (x == 0)
        {
            isHorizontalAxisInUse = false;
        }

        if (!isVerticalAxisInUse && y != 0)
        {
            if (y > 0.5f)
            {
                dir = Vector2Int.up;
            }
            else if (y < -0.5f)
            {
                dir = Vector2Int.down;
            }
            SoundManager.Instance.PlaySound(moveSound);
            isVerticalAxisInUse = true;
        }
        else if (y == 0)
        {
            isVerticalAxisInUse = false;
        }

        // --- 矢印キー入力で移動 ---
        //if (Input.GetKeyDown(KeyCode.UpArrow)) dir = Vector2Int.up;
        //if (Input.GetKeyDown(KeyCode.DownArrow)) dir = Vector2Int.down;
        //if (Input.GetKeyDown(KeyCode.LeftArrow)) dir = Vector2Int.left;
        //if (Input.GetKeyDown(KeyCode.RightArrow)) dir = Vector2Int.right;


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
        if (Input.GetButtonDown("Submit"))
        {
            currentNode.Press();
        }
    }
}
