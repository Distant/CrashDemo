using UnityEngine;
using System.Collections;

public class FrictionLessSlope : MonoBehaviour {

	public float Speed;
	private bool open = false;
	public bool playerInTrigger;
	private Animator anim; 

	[SerializeField]
	private float animOffset;
	[SerializeField]
	private float openTime;
	[SerializeField]
	private float closeTime;

	[SerializeField]
	private BoxCollider[] colliders;

	private CharacterControl player; 

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterControl> ();
		StartCoroutine (Switch ());
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetStairOpen(bool b){
		open = b ;
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Player") {
			if (!open) {
			 
				player.OnSlippingSlope ();
			
			}
			playerInTrigger = true;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.tag == "Player") {
			if (!open) {
					player.OffSlippingSlope ();
			}
			playerInTrigger = false;
		}
	}

	public IEnumerator Switch(){
		yield return new WaitForSeconds (animOffset);
		while (true) {
			open = true;
			if (playerInTrigger) player.OffSlippingSlope ();
			GetComponent<BoxCollider> ().enabled = false;
			foreach (BoxCollider col in colliders){
				col.enabled = true;
			}
			anim.SetBool("open", true);
			yield return new WaitForSeconds(openTime);

			anim.SetBool("open", false);
			yield return new WaitForSeconds(0.15f);
			GetComponent<BoxCollider> ().enabled = true;
			foreach (BoxCollider col in colliders){
				col.enabled = false;
			}
			open = false;
			if (playerInTrigger) player.OnSlippingSlope ();
			yield return new WaitForSeconds(closeTime - 0.15f);
		}
	}
}
