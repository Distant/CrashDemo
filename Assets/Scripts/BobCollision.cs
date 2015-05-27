using UnityEngine;
using System.Collections;

public class BobCollision : MonoBehaviour {

	public BobAndFall parent;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider col){
		if (col.tag == "Player"){
			parent.fall ();
		}
	}
}