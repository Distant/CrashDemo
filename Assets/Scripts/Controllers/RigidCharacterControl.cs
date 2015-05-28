using UnityEngine;
using System.Collections;

public class RigidCharacterControl : MonoBehaviour, CharacterControl
{
	public Transform playerModel;
	public Vector3 direction = Vector3.forward;
	public int state = 0;
	public Vector3 initialPosition;
	private Rigidbody rigidBody;
	private LevelManager manager;
	Vector3 velocity = Vector3.zero;
	private float moveSpeed = 2;
	private float jumpMoveSpeed = 2;

	public float height { get; private set; }
	public bool Spinning { get; private set; }

	private float jumpSpeed = 6f;
	private float distToGround;
	// Use this for initialization
	void Start ()
	{
		initialPosition = transform.position;
		manager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();
		rigidBody = GetComponent<Rigidbody> ();
		distToGround = GetComponentInChildren<Collider> ().bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void FixedUpdate ()
	{
		velocity.y = rigidBody.velocity.y;
		velocity = new Vector3 (Input.GetAxis ("Horizontal"), rigidBody.velocity.y, Input.GetAxis ("Vertical"));

		velocity = transform.TransformDirection (velocity);
		velocity.x *= IsGrounded ? moveSpeed : jumpMoveSpeed;
		velocity.z *= IsGrounded ? moveSpeed : jumpMoveSpeed;
			
		if (Input.GetButton ("Jump") && IsGrounded) {
			Jump (jumpSpeed);
		}
		
		rigidBody.velocity = velocity;
		
		if (Input.GetKeyDown (KeyCode.LeftShift) && !Spinning)
			Spin ();
		
		if (transform.position.y < -2) {
			Die ();
		}
	}
	
	public void Jump (float speed)
	{
		velocity.y = speed;
		if ((velocity.z != 0 || velocity.x != 0) && !Spinning) {
			StartCoroutine (Flip (new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"))));
		}
	}
	
	public IEnumerator Flip (Vector3 dir)
	{
		yield return new WaitForSeconds (0.1f);
		Vector3 targetDir = (Quaternion.Euler (0, 90, 0) * dir).normalized;
		while (true) {
			if (IsGrounded || Spinning) {
				playerModel.transform.rotation = Quaternion.identity;
				break;
			}
			playerModel.transform.Rotate (targetDir, 10);
			yield return new WaitForEndOfFrame ();
		}
	}

	public void Spin ()
	{
		Spinning = true;
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
		Vector3 rot = playerModel.transform.rotation.eulerAngles;
		playerModel.transform.rotation = Quaternion.Euler (0, 0, 0);
		Spinning = false;
	}

	public void Die ()
	{
		transform.position = initialPosition;
		manager.death ();
	}

	public bool IsGrounded
	{
		get {return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.1f) || rigidBody.velocity.y == 0;}
	}
	public void Stop(){
	}

}
