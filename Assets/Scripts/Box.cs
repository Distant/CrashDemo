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
			if (col.transform.position.y > transform.position.y + 0.5f) {
				if (col.GetComponentInParent<CharacterControl> ().Spinning) {
					col.GetComponentInParent<CharacterControl> ().Stop ();
				} else
					col.GetComponentInParent<CharacterControl> ().Jump (5f);
				inventoryManager.addWumpas (5);
				gameObject.SetActive (false);
			}
			else if (col.GetComponentInParent<CharacterControl> ().Spinning) {
				inventoryManager.addWumpas (5);
				gameObject.SetActive (false);
			}
		}
	}
}
