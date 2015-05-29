using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject BoxesObj;
	private Box[] boxes;
	private InventoryManager inventoryManager;

	// Use this for initialization
	void Start () {
		boxes = BoxesObj.GetComponentsInChildren<Box>();
		inventoryManager = gameObject.GetComponent<InventoryManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void death(){
		inventoryManager.death ();
		foreach (Box box in boxes) {
			box.gameObject.SetActive(true);
		}
	}

	public void EndLevel(){
		inventoryManager.VerifyBoxCount ();
	}
}