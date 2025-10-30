 
using  UnityEngine;
using System.Collections;
using UnityEngine.UI;

//　スキルのタイプ
public enum SkillType
{
	HP1,
	HP2,
	HP3,
	Attack1,
	Attack2,
	Attack3,
	Shot1,
	Shot2,
	Special1,
	Special2,
	Special3,
	Junior1,
	Junior2,
	Junior3,
	Junior4,
	Junior5,
	Junior6,
	Passive1
};

public class SkillSystem : MonoBehaviour
{
	//  プレイヤーコントローラー
	public PlayerController player;
	//　スキルを覚える為のスキルポイント
	[SerializeField] private int skillPoint;
	//　スキルを覚えているかどうかのフラグ
	[SerializeField] private bool[] skills;
	//　スキル毎のパラメータ
	[SerializeField] private SkillParam[] skillParams;
	//　スキルポイントを表示するテキストUI
	public Text skillText;

	void Awake()
	{
		//　スキル数分の配列を確保
		skills = new bool[skillParams.Length];
		SetText();
	}
	//　スキルを覚える
	public void LearnSkill(SkillType type, int point)
	{
		skills[(int)type] = true;
		SetSkillPoint(point);
		SetText();
		CheckOnOff();
	}
	//　スキルを覚えているかどうかのチェック
	public bool IsSkill(SkillType type)
	{
		return skills[(int)type];
	}
	//スキルポイントを取得する
	public void TakeSkillPoint(int point)
	{
		skillPoint += point;
		InformationUIController.Instance.UpdateSkillPoint(skillPoint);
	}
	//　スキルポイントを減らす
	public void SetSkillPoint(int point)
	{
		skillPoint -= point;
		InformationUIController.Instance.UpdateSkillPoint(skillPoint);
	}
	//　スキルポイントを取得
	public int GetSkillPoint()
	{
		return skillPoint;
	}
	//　スキルを覚えられるかチェック
	public bool CanLearnSkill(SkillType type, int spendPoint = 0)
	{
		//　持っているスキルポイントが足りない
		if (skillPoint < spendPoint)
		{
			return false;
		}
		if (type == SkillType.Attack2)
		{
			return skills[(int)SkillType.Attack1];
		}
		else if (type == SkillType.Attack3)
		{
			return skills[(int)SkillType.Attack2];
		}
		else if (type == SkillType.HP3)
		{
			return skills[(int)SkillType.HP2];
		}
		else if (type == SkillType.HP2)
		{
			return skills[(int)SkillType.HP1];
		}
		else if (type == SkillType.Shot2)
		{
			return skills[(int)SkillType.Shot1];
		}
		else if (type == SkillType.Special2)
		{
			return skills[(int)SkillType.Special1];
		}
		else if (type == SkillType.Junior4)
		{
			return skills[(int)SkillType.Junior3];
		}
		return true;
	}
	//　スキル毎にボタンのオン・オフをする処理を実行させる
	void CheckOnOff()
	{
		foreach (var skillParam in skillParams)
		{
			skillParam.CheckButtonOnOff();
		}
	}

	public void SetText()
	{
		skillText.text = "スキルポイント：" + skillPoint;
	}
}
