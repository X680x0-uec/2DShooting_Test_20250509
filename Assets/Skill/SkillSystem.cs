 
using  UnityEngine;
using System.Collections;
using UnityEngine.UI;

//　スキルのタイプ
public enum SkillType
{
	HP,
	Attack,
	Speed
};
 
public class SkillSystem : MonoBehaviour {
    //　スキルを覚える為のスキルポイント
    [SerializeField] private int skillPoint;
    //　スキルを覚えているかどうかのフラグ
    [SerializeField] private bool[] skills;
    //　スキル毎のパラメータ
    [SerializeField] private SkillParam[] skillParams;
    //　スキルポイントを表示するテキストUI
    public Text skillText;
 
	void Awake () {
		//　スキル数分の配列を確保
		skills = new bool[skillParams.Length];
		SetText ();
	}
	//　スキルを覚える
	public void LearnSkill(SkillType type, int point) {
		skills [(int)type] = true;
		SetSkillPoint (point);
		SetText ();
		CheckOnOff ();
	}
	//　スキルを覚えているかどうかのチェック
	public bool IsSkill(SkillType type) {
		return skills [(int)type];
	}
	//　スキルポイントを減らす
	public void SetSkillPoint(int point) {
		skillPoint -= point;
	}
	//　スキルポイントを取得
	public int GetSkillPoint() {
		return skillPoint;
	}
	//　スキルを覚えられるかチェック
	public bool CanLearnSkill(SkillType type, int spendPoint = 0) {
		//　持っているスキルポイントが足りない
		if (skillPoint < spendPoint) {
			return false;
		}
        return true;
	}
	//　スキル毎にボタンのオン・オフをする処理を実行させる
	void CheckOnOff() {
		foreach (var skillParam in skillParams) {
			skillParam.CheckButtonOnOff ();
		}
	}
 
	void SetText() {
		skillText.text = "スキルポイント：" + skillPoint;
	}
}
