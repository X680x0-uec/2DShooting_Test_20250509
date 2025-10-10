 
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
	void Update () {
		if (Input.GetKeyDown("q"))
		{
			skillSystem.GetComponent<SkillSystem>().SetText();
			skillSystem.SetActive(!skillSystem.activeSelf);
			EventSystem.current.SetSelectedGameObject(firstSelect);
		}
	}
}
 