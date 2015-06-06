using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{
    protected LevelManager levelManager;
	protected CharacterControl player;
	protected SoundManager soundManager;

	// Use this for initialization
	public virtual void Start ()
	{
		levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControl>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public virtual void HitPlayer(GameObject g){
		if (g.transform.position.y > transform.position.y + 0.5f && (player.Velocity.y < -2f || player.Jumping)) {
			if (player.Spinning) {
				player.Stop ();
			} else{
			    player.Jump (7f);}
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
		soundManager.PlayClipAtPoint ("box_break", transform.position, 0.2f);
		levelManager.InventoryManager.BreakBox (5);
		gameObject.SetActive (false);
        player.NotTouching(this);
    }
}