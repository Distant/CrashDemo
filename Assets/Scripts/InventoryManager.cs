using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

	private int wumpas;
	private int tempwumpas;
	private int lives = 5;
	public Text wumpaText;
	private bool counting;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int Wumpas(){
		return wumpas;
	}

	public void addWumpas(int w){
		wumpas += w;
		if (wumpas > 100) {
			addLives(1);
			wumpas = wumpas - 100;
		}
		if (!counting) {
			counting = true;
			StartCoroutine (UpdateWumpaText ());
		}
	}

	public IEnumerator UpdateWumpaText() {
		while(tempwumpas < wumpas) {
			tempwumpas += 1;
			wumpaText.text = tempwumpas.ToString ();
			yield return new WaitForSeconds(0.2f);
		}
		counting = false;
		yield return new WaitForSeconds(0);
	}

	public void addLives(int l){
		lives += l;
		if (lives > 99) {
			lives = 99;
			return;
		}
		if (lives < 0) {
			lives = 0;
			return;
		}
	}

	public int getLives(){
		return lives;
	}

	public void death(){
		wumpas = 0;
		tempwumpas = 0;
		addLives (-1);
		wumpaText.text = tempwumpas.ToString ();
	}
}
