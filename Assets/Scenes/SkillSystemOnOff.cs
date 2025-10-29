 
using  UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
 
public class SkillSystemOnOff : MonoBehaviour {

	[SerializeField]
	public GameObject skillSystem;


	//　最初にフォーカスするゲームオブジェクト
	[SerializeField]
	public GameObject firstSelect;
 
	// Update is called once per frame
	int opencount = 0;
	void Update () {
		if (Input.GetKeyDown("q"))
		{
			if (opencount%2 == 0)
			{
				Time.timeScale = 0f;
				opencount += 1;
			}
            else
            {
				Time.timeScale = 1f;
				opencount += 1;
            }
			skillSystem.GetComponent<SkillSystem>().SetText();
			skillSystem.SetActive(!skillSystem.activeSelf);
			EventSystem.current.SetSelectedGameObject(firstSelect);
		}
	}
}
 