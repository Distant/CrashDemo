using UnityEngine;
using System.Collections;

public class Wumpa : MonoBehaviour {

	private LevelManager levelManager;

	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent < LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Camera.main.transform.position, Vector3.up);
	}


	public void OnTriggerEnter(Collider col){
		if (col.tag == "Player") {
			levelManager.InventoryManager.AddWumpa (transform);
			Destroy(this.gameObject);
		}
	}
}
