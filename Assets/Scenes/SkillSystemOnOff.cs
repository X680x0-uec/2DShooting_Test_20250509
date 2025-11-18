 
using  UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
 
public class SkillSystemOnOff : MonoBehaviour {
	public AudioClip opensound;

	[SerializeField]
	public GameObject skillSystem;

	public static bool IsCheckingSkill { get; private set; } = false;


	//　最初にフォーカスするゲームオブジェクト
	[SerializeField]
	public GameObject firstSelect;
 
	// Update is called once per frame
	int opencount = 0;
	void Update () {
		if (Input.GetButtonDown("OpenSkillPanel") && !PlayerController.IsGameOverOrGameClear)
		{
			if (opencount%2 == 0)
			{
				SoundManager.Instance.PlaySound(opensound);
				Time.timeScale = 0f;
				IsCheckingSkill = true;
				opencount += 1;
			}
            else
            {
				Time.timeScale = 1f;
				IsCheckingSkill = false;
				opencount += 1;
            }
			skillSystem.GetComponent<SkillSystem>().SetText();
			skillSystem.SetActive(!skillSystem.activeSelf);
			EventSystem.current.SetSelectedGameObject(firstSelect);
		}
		else if (Input.GetButtonDown("Cancel") && opencount%2 == 1 && !PlayerController.IsGameOverOrGameClear)
		{
			Time.timeScale = 1f;
			IsCheckingSkill = false;
			opencount += 1;

			skillSystem.GetComponent<SkillSystem>().SetText();
			skillSystem.SetActive(!skillSystem.activeSelf);
			EventSystem.current.SetSelectedGameObject(firstSelect);
		}
	}

	public void CloseSkillPanel()
    {
        Time.timeScale = 1f;
		IsCheckingSkill = false;
		opencount = 0;
		skillSystem.GetComponent<SkillSystem>().SetText();
		skillSystem.SetActive(false);
		EventSystem.current.SetSelectedGameObject(firstSelect);
    }
}
 