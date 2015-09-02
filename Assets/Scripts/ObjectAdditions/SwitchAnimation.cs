using UnityEngine;
using System.Collections;

public class SwitchAnimation : MonoBehaviour {

	private Animator anim;
	private bool active;
	[SerializeField] private float activeTime;
	[SerializeField] private float inactiveTime;
	[SerializeField] private float animOffset;

	private float animTime;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		animTime = anim.runtimeAnimatorController.animationClips [0].length;
		StartCoroutine (Animate ());
	}
	
	// Update is called once per frame
	void Update () {
	}

	public IEnumerator Animate(){
		yield return new WaitForSeconds (animOffset + animTime);
		while (true) { 
			if (active) {
				anim.SetBool ("active", active);
				active = false;
				yield return new WaitForSeconds (activeTime);
			} else {
				anim.SetBool ("active", active);
				active = true;
				yield return new WaitForSeconds (inactiveTime);
			}
		}
	}
}
