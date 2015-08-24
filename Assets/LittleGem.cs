using UnityEngine;
using System.Collections;

public class LittleGem : MonoBehaviour {

	private Transform player;
	float dist;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		dist = Vector3.Distance (transform.position, player.transform.position);
		if (dist < 1f) {
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 3 * Time.deltaTime * Mathf.Sqrt(1/ (dist)));
		}
	}

	public void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player")
			Destroy (this.gameObject);
	}
}
