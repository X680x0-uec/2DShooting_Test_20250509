using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
public Sprite _on; // 表示する画像1
public Sprite _off; // 表示する画像2
private bool isOn = true;

public SkillSystem skillsystem;
private SkillParam skillParam;

public SkillType type;

private int spendPoint;

    void Start()
    {
        skillParam = GetComponentInParent<SkillParam>();
        spendPoint = skillParam.spendPoint;
    }

    public void ChangeImageSprite()
    {
        if (skillsystem.CanLearnSkill(type,spendPoint))
        {
            Debug.Log("aaaaa");
            var img = GetComponent<Image>();
            img.sprite = isOn ? _on : _off; // 状態に応じて画像を切り替え
            isOn = true; // 状態を反転;
        }

    }
}