using UnityEngine;
using System.Collections;

public class BobAndFall : MonoBehaviour {

	private bool falling = false;
	public GameObject child;
	private Vector3 initialPosition;
	private float bobMax = 3f;
	private float bobSmall = 0.5f;
	private float speed = 12f;
	private float speedFall = 5f;
	private float speedRise = 8f;
	private bool touching;

	// Use this for initialization
	void Start () {
		initialPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fall() {
		if (!falling) {
			falling = true;
			StartCoroutine (fallRoutine ());
		} else
			touching = true;
	}

	public IEnumerator fallRoutine() {
		Vector3 currentPosition = initialPosition;
		while (true) {
			currentPosition = Vector3.Lerp(currentPosition, initialPosition - new Vector3(0, bobSmall, 0), speed * Time.deltaTime);
			transform.position = currentPosition;
			if (currentPosition.y < initialPosition.y - bobSmall + 0.05f) break;
			yield return new WaitForEndOfFrame();
		}
		while (true) {
			currentPosition = Vector3.Lerp(currentPosition, initialPosition, speed * Time.deltaTime);
			transform.position = currentPosition;
			if (currentPosition.y > initialPosition.y - 0.1f) break;
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds (0.4f);
		while (true) {
			currentPosition = Vector3.Lerp (currentPosition, initialPosition - new Vector3 (0, bobMax, 0), speedFall * Time.deltaTime);
			transform.position = currentPosition;
			if (currentPosition.y < initialPosition.y - bobMax + 0.05f)
				break;
			yield return new WaitForEndOfFrame ();
		}
		yield return new WaitForSeconds (1f);
		touching = false;
		while (true) {
			currentPosition = Vector3.Lerp(currentPosition, initialPosition, speedRise * Time.deltaTime);
			transform.position = currentPosition;
			if (currentPosition.y > initialPosition.y - 0.1f) break;
			yield return new WaitForEndOfFrame();
		}
		falling = false;
		if (touching) {
			touching  = false;
			fall ();
		}
	}
}