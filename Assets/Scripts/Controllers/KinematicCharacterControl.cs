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
	Vector3 velocity = Vector3.zero;
	private float moveSpeed = 2f;
	private float jumpMoveSpeed = 2;

	public float height { get; private set;}
	public bool Spinning { get; private set;}

	private float jumpSpeed = 6f;
	private float terminalVelocity = -6f;

	private List<Box> touching = new List<Box>();

	private bool flipping;
	// Use this for initialization
	void Start ()
	{
		initialPosition = transform.position;
		manager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager>();
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (controller.isGrounded || !(Input.GetAxis ("Horizontal") == 0) || ! (Input.GetAxis ("Vertical") == 0)) {
			velocity = new Vector3 (Input.GetAxis ("Horizontal"), velocity.y, Input.GetAxis ("Vertical"));
			velocity = transform.TransformDirection (velocity);
			velocity.x *= controller.isGrounded ? moveSpeed : jumpMoveSpeed;
			velocity.z *= controller.isGrounded ? moveSpeed : jumpMoveSpeed;
			
			if (Input.GetButton ("Jump") && controller.isGrounded) {
				Jump(jumpSpeed);
			}
		}
		
		// Apply gravity
		if (!controller.isGrounded && velocity.y > terminalVelocity) velocity.y -= 16* Time.deltaTime;
		
		// Move the controller
		controller.Move(velocity * Time.deltaTime);
		
		if (Input.GetKeyDown (KeyCode.LeftShift) && !Spinning)
			Spin ();
		
		if (transform.position.y < -2) {
			Die();
		}
	}

	public void Jump(float speed){
		velocity.y = speed;
		if ((velocity.z != 0 || velocity.x !=0) && !Spinning &&!flipping) {
			StartCoroutine(Flip(new Vector3(Input.GetAxis ("Horizontal"),0, Input.GetAxis ("Vertical"))));
		}
	}

	public IEnumerator Flip(Vector3 dir){
		yield return new WaitForEndOfFrame();
		flipping = true;
		Vector3 targetDir = (Quaternion.Euler (0, 90, 0) * dir).normalized;
		while(true){
			if (controller.isGrounded || Spinning) {
				playerModel.transform.rotation = Quaternion.identity;
				flipping = false;
				break;
			}
			playerModel.transform.Rotate(targetDir, 8);
			yield return new WaitForEndOfFrame();
		}
	}

	public void Spin (){
		Spinning = true;
		foreach (Box box in touching) {
			box.Remove();
		}
		touching.Clear ();
		StartCoroutine (SpinAnim ());
	}

	public IEnumerator SpinAnim(){
		float countdown = 0.5f;
		while (true) {
			countdown -= Time.deltaTime;
			if (countdown < 0){
				break;
			}
			playerModel.transform.Rotate(new Vector3(0,1,0), 16);
			yield return new WaitForEndOfFrame();
		}
		Vector3 rot = playerModel.transform.rotation.eulerAngles;
		playerModel.transform.rotation = Quaternion.Euler(0, 0, 0);
		Spinning = false;
	}

	public void Die(){
		transform.position = initialPosition;
		manager.death ();
	}

	public void Stop(){
		velocity.y = 0;
	}

	public void Touching(Box box){
		touching.Add (box);
	}

	public void NotTouching(Box box){
		if (touching.Contains(box))
		touching.Remove (box);
	}
}
