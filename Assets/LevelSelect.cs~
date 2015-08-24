using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	private GameManager game;
	public LevelNode[] levelList;
	private int selectedIndex;

	// Use this for initialization
	void Start () {
		game = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		for (int i = 0; i < Mathf.Min(game.LevelsUnlocked, levelList.Length); i++) {
			levelList[i].gameObject.SetActive(true);
		}
		levelList [selectedIndex].Select ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.D)) {
			levelList[selectedIndex].Deselect();
			IncreaseIndex(ref selectedIndex, game.LevelsUnlocked);
			levelList[selectedIndex].Select() ;
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			levelList[selectedIndex].Deselect();
			DecreaseIndex(ref selectedIndex, game.LevelsUnlocked);
			levelList[selectedIndex].Select();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			GameObject o = GameObject.FindGameObjectWithTag("GameManager");
			if (o != null) o.GetComponent<GameManager>().LoadLevel(selectedIndex + 2);
		}
	}

	public void IncreaseIndex(ref int index, int total){
		index ++;
		if (index > total - 1) index = 0;
	}

	public void DecreaseIndex(ref int index, int total){
		index --;
		if (index < 0) index = total - 1;
	}
}