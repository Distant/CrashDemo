using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{
	private InventoryManager inventoryManager;

	// Use this for initialization
	void Start ()
	{
		inventoryManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<InventoryManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void OnTriggerEnter (Collider col)
	{
		if (col.tag == "Player") {
			if (false)
				col.GetComponentInParent<CharacterControl> ().Jump (5f);
			if (true && col.GetComponentInParent<CharacterControl> ().Spinning) { // if hit from top, or side and spinning
				inventoryManager.addWumpas (5);
				gameObject.SetActive (false);
			}
		}
	}
}
