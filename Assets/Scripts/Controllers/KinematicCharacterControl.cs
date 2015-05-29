using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KinematicCharacterControl : MonoBehaviour, CharacterControl
{
	public Transform playerModel;
	CharacterController controller;
	public Vector3 direction = Vector3.forward;
	public int state = 0;
	public Vector3 initialPosition;
	private Rigidbody rigidBody;
	private LevelManager manager;
	private Vector3 velocity;

	public Vector3 Velocity { get { return velocity; } private set { velocity = value; } }

	private float moveSpeed = 2f;
	private float jumpMoveSpeed = 2;

	public bool Jumping { get; private set; }

	public float height { get; private set; }

	public bool Spinning { get; private set; }

	private float jumpSpeed = 6f;
	private float terminalVelocity = -6f;
	private List<Box> touching = new List<Box> ();
	private bool flipping;
	// Use this for initialization
	void Start ()
	{
		velocity = Vector3.zero;
		initialPosition = transform.position;
		manager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (controller.isGrounded || !(Input.GetAxis ("Horizontal") == 0) || ! (Input.GetAxis ("Vertical") == 0)) {
			if (controller.isGrounded && !Jumping)
				velocity.y = 0;
			velocity = new Vector3 (Input.GetAxis ("Horizontal"), velocity.y, Input.GetAxis ("Vertical"));
			velocity = transform.TransformDirection (velocity);
			velocity.x *= controller.isGrounded ? moveSpeed : jumpMoveSpeed;
			velocity.z *= controller.isGrounded ? moveSpeed : jumpMoveSpeed;
			
			if (Input.GetButton ("Jump") && controller.isGrounded) {
				Jump (jumpSpeed);
			}
		}
		
		// Apply gravity
		if (velocity.y > terminalVelocity)
			velocity.y -= 16 * Time.deltaTime;

		// Move the controller
		if (controller.isGrounded && velocity.y <= 0) {
			Jumping = false;
		}

		controller.Move (velocity * Time.deltaTime);

		if (Input.GetKeyDown (KeyCode.LeftShift) && !Spinning) {
			Spin ();
		}
		
		if (transform.position.y < -2) {
			Die ();
		}
	}

	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.collider.tag == "Box") {
			if (hit.collider.GetComponent<Box> ().gameObject.activeSelf)
			hit.collider.GetComponent<Box> ().HitPlayer (this.gameObject);
		} else if (hit.collider.tag == "EndTrigger"){
			manager.EndLevel();
			hit.collider.gameObject.SetActive(false);
		}
	}

	public void Jump (float speed)
	{
		Jumping = true;
		velocity.y = speed;
		//StartCoroutine (Flip (new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"))));
	}

	public IEnumerator Flip (Vector3 dir)
	{
		yield return new WaitForSeconds (0.3f);
		if ((Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) && !Spinning && !flipping) {
			flipping = true;
			Vector3 targetDir = (Quaternion.Euler (0, 90, 0) * dir).normalized;
			while (true) {
				if ((controller.isGrounded) || Spinning) {
					playerModel.transform.rotation = Quaternion.identity;
					flipping = false;
					break;
				}
				playerModel.transform.Rotate (targetDir, 12f);
				yield return new WaitForEndOfFrame ();
			}
		} else
			yield return new WaitForSeconds (0);
	}

	public void Spin ()
	{
		Spinning = true;
		foreach (Box box in touching) {
			if (box.gameObject.activeSelf)
			box.Break ();
		}
		touching.Clear ();
		StartCoroutine (SpinAnim ());
	}

	public IEnumerator SpinAnim ()
	{
		float countdown = 0.5f;
		while (true) {
			countdown -= Time.deltaTime;
			if (countdown < 0) {
				break;
			}
			playerModel.transform.Rotate (new Vector3 (0, 1, 0), 16);
			yield return new WaitForEndOfFrame ();
		}
		playerModel.transform.rotation = Quaternion.Euler (0, 0, 0);
		Spinning = false;
	}

	public void Die ()
	{
		transform.position = initialPosition;
		manager.death ();
	}

	public void Stop ()
	{
		velocity.y = 0;
	}

	public void Touching (Box box)
	{
		if (!touching.Contains (box))
			touching.Add (box);
	}

	public void NotTouching (Box box)
	{
		if (touching.Contains (box))
			touching.Remove (box);
	}
}
