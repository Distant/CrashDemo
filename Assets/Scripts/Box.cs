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
			CharacterControl player = col.GetComponentInParent<CharacterControl>();

			if (col.transform.position.y > transform.position.y + 0.5f) {
				if (player.Spinning) {
					player.Stop ();
				} else
					player.Jump (5f);
				Remove ();
			}
			else if (player.Spinning) {
				Remove();
			} else {
				player.Touching(this);
			}
		}
	}
	public void OnTriggerExit (Collider col){
		if (col.tag == "Player") {
			CharacterControl player = col.GetComponentInParent<CharacterControl> ();
			player.NotTouching(this);
		}
	}

	public void Remove(){
		inventoryManager.addWumpas (5);
		gameObject.SetActive (false);
	}
}
