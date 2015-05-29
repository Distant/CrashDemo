using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{
	private InventoryManager inventoryManager;
	private CharacterControl player;

	// Use this for initialization
	void Start ()
	{
		inventoryManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<InventoryManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void HitPlayer(GameObject g){
		player = g.GetComponent<CharacterControl>();
		if (g.transform.position.y > transform.position.y + 0.5f && (player.Velocity.y < -2f || player.Jumping)) {
			if (player.Spinning) {
				player.Stop ();
			} else{
			    player.Jump (5f);}
			Break ();
		}
		else if (player.Spinning) {
			Break();
		} 
	}

	public void OnTriggerEnter (Collider col){
		if (col.tag == "Player") {
			CharacterControl player = col.GetComponentInParent<CharacterControl> ();
			player.Touching(this);
		}
	}

	public void OnTriggerExit (Collider col){
		if (col.tag == "Player") {
			CharacterControl player = col.GetComponentInParent<CharacterControl> ();
			player.NotTouching(this);
		}
	}

	public void Break(){
		inventoryManager.BreakBox (5);
		gameObject.SetActive (false);
	}
}