using UnityEngine;
using System.Collections;

public class BobAndFall : MonoBehaviour {

	private bool falling = false;
	public GameObject child;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fall(){
		if (!falling) {
			falling = true;
			StartCoroutine (fallRoutine ());
		}
	}

	public IEnumerator fallRoutine(){
		yield return new WaitForSeconds (0.4f);
		child.SetActive (false);
		yield return new WaitForSeconds (1f);
		child.SetActive (true);
		falling = false;
	}
}