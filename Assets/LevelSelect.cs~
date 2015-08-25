using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	private GameManager game;
	public LevelNode[] levelList;
	private int selectedIndex;

	// Use this for initialization
	void Start () {
		game = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		for (int i = levelList.Length - 1; i > Mathf.Max(game.LevelsUnlocked - 1, 0); i--) {
			levelList[i].DisableNode();
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
			game.LoadLevel(selectedIndex + 2);
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