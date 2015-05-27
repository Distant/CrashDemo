using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour
{

	public Vector3 direction = Vector3.forward;
	public float height;
	public int state = 0;
	public Vector3 initialPosition;
	private Rigidbody rigidBody;
	private LevelManager manager;
	Vector3 velocity = Vector3.zero;
	private float moveSpeed = 2;
	private float jumpMoveSpeed = 2;

	private float jumpSpeed = 6f;
	// Use this for initialization
	void Start ()
	{
		initialPosition = transform.position;
		//rigidBody = this.GetComponent<Rigidbody> ();
		manager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		CharacterController controller = GetComponent<CharacterController> ();
		if (controller.isGrounded || !(Input.GetAxis ("Horizontal") == 0) || ! (Input.GetAxis ("Vertical") == 0)) {
			velocity = new Vector3 (Input.GetAxis ("Horizontal"), controller.velocity.y, Input.GetAxis ("Vertical"));
			velocity = transform.TransformDirection (velocity);
			velocity.x *= controller.isGrounded ? moveSpeed : jumpMoveSpeed;
			velocity.z *= controller.isGrounded ? moveSpeed : jumpMoveSpeed;
			
			if (Input.GetButton ("Jump") && controller.isGrounded) {
				velocity.y = jumpSpeed;
			}
		}
		// Apply gravity
		velocity.y -= 16* Time.deltaTime;
		
		// Move the controller
		controller.Move(velocity * Time.deltaTime);

		
		if (transform.position.y < -2) {
			Die();
		}
	}

	public void OnCollisionEnter (Collision col) // no longer using rigidbody
	{
		if (col.contacts.Length > 0) {
			ContactPoint[] c = col.contacts;
			if (Vector3.Dot (c [0].normal, Vector3.up) > 0.5f) {
				if (col.gameObject.tag == "Box"){
					state = 1;
					Vector3 velocity = rigidBody.velocity;
					velocity.y = 5f;
				} else {
					state = 0;
				}
			}
		}
	}

	public void Die(){
		transform.position = initialPosition;

		manager.death ();
	}
}
