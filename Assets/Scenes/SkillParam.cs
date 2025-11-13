 
using  UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
 
public class SkillParam : MonoBehaviour {
    //　スキル管理システム
    [SerializeField]
    private SkillSystem skillSystem;
    //　このスキルの種類
    [SerializeField]
    private SkillType type;
    //　このスキルを覚える為に必要なスキルポイント
    [SerializeField]
    private int spendPoint;
    //　スキルのタイトル
    [SerializeField]
    private string skillTitle;
    //　スキル情報
    [SerializeField]
    private string skillInformation;
	//　スキル情報を載せるテキストUI
	[SerializeField]
	private Text text;
	public ChangeImage image;
 
	// Use this for initialization
	void Start () {
		//　スキルを覚えられる状態でなければボタンを無効化
		CheckButtonOnOff ();
	}
 
	//　スキルボタンを押した時に実行するメソッド
	public void OnClick() {
		//　スキルを覚えていたら何もせずreturn
		if (skillSystem.IsSkill (type)) {
			return;
		}
		//　スキルを覚えられるかどうかチェック
		if (skillSystem.CanLearnSkill (type, spendPoint)) {

			image.ChangeImageSprite();
			//　スキルを覚えさせる
			skillSystem.LearnSkill (type, spendPoint);
 
			ChangeButtonColor (new Color(0f, 0f, 1f, 1f));

			switch (type)
			{
				case SkillType.HP1: skillSystem.player.GetSkill("HP", 0, 1); break;
				case SkillType.HP2: skillSystem.player.GetSkill("HP", 0, 2); break;
				case SkillType.HP3: skillSystem.player.GetSkill("HP", 0, 3); break;
				case SkillType.Attack1: skillSystem.player.GetSkill("Attack", 0, 1); break;
				case SkillType.Attack2: skillSystem.player.GetSkill("Attack", 0, 2); break;
				case SkillType.Attack3: skillSystem.player.GetSkill("Attack", 0, 3); break;
				case SkillType.Shot1: skillSystem.player.GetSkill("Shot", 0, 1); break;
				case SkillType.Shot2: skillSystem.player.GetSkill("Shot", 0, 2); break;
				case SkillType.Special1: skillSystem.player.GetSkill("Special", 0, 1); break;
				case SkillType.Special2: skillSystem.player.GetSkill("Special", 0, 2); break;
				case SkillType.Special3: skillSystem.player.GetSkill("Special", 1, 1); break;
				case SkillType.Special4: skillSystem.player.GetSkill("Special", 2, 1); break;
				case SkillType.Special5: skillSystem.player.GetSkill("Special", 2, 2); break;
				case SkillType.Special6: skillSystem.player.GetSkill("Special", 2, 3); break;
				case SkillType.Junior1: skillSystem.player.GetSkill("Junior", 0, 1); break;
				case SkillType.Junior2: skillSystem.player.GetSkill("Junior", 1, 1); break;
				case SkillType.Junior3: skillSystem.player.GetSkill("Junior", 2, 1); break;
				case SkillType.Junior4: skillSystem.player.GetSkill("Junior", 2, 2); break;
				case SkillType.Junior5: skillSystem.player.GetSkill("Junior", 3, 1); break;
				case SkillType.Junior6: skillSystem.player.GetSkill("Junior", 4, 1); break;
				case SkillType.Passive1: skillSystem.player.GetSkill("Passive", 0, 1); break;
				case SkillType.Passive2: skillSystem.player.GetSkill("Passive", 1, 1); break;
			}
			
 
			text.text = skillTitle + "を覚えた";
		} else {
			text.text = "スキルを覚えられません。";
		}
	}
 
	//　他のスキルを習得した後の自身のボタンの処理
	public void CheckButtonOnOff () {
		//　スキルを覚えられるかどうかチェック
		if (!skillSystem.CanLearnSkill (type)) {
			ChangeButtonColor (new Color (0.8f, 0.8f, 0.8f, 0.8f));
		//　スキルをまだ覚えていない
		} else if(!skillSystem.IsSkill (type)) {
			ChangeButtonColor (new Color (1f, 1f, 1f, 1f));
		}
	}
	//　スキル情報を表示
	public void SetText() {
		text.text = skillTitle + "：消費スキルポイント" + spendPoint + "\n" + skillInformation;
	}
	//　スキル情報をリセット
	public void ResetText() {
		text.text = "";
	}
	//　ボタンの色を変更する
	public void ChangeButtonColor(Color color) {
		//　ボタンコンポーネントを取得
		Button button = gameObject.GetComponent <Button> ();
		//　ボタンのカラー情報を取得（一時変数を作成し、色情報を変えてからそれをbutton.colorsに設定しないとエラーになる）
		ColorBlock cb = button.colors;
        //　取得済みのスキルボタンの色を変える
        cb.normalColor = color;
		cb.pressedColor = color;
		//　ボタンのカラー情報を設定
		button.colors = cb;
	}
}
