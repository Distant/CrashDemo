using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(CharacterController))]
public class CharacterControl : MonoBehaviour
{
	private static readonly string PLAYER_HOP = "player_hop";
	private static readonly string PLAYER_SPIN = "player_spin";
	private static readonly float SPIN_LENGTH = 0.5f;
	[SerializeField]
	private Transform playerModel;
	private CharacterController controller;
	private LevelManager levelManager;
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private float jumpSpeed = 2f;
	[SerializeField] private float terminalVelocity = -8f;
	[SerializeField] private float gravity = 24;
	private Vector3 initialPosition;
	private Vector3 velocity = Vector3.zero;

	public Vector3 Velocity { get { return velocity; } private set { velocity = value; } }
	
	public bool Jumping { get; private set; }

	public float height { get; private set; }

	public bool Spinning { get; private set; }

	public List<Box> touching = new List<Box> ();
	public Transform g;
	private Vector3 slope;
	private bool canJump;
	private bool lowGravity;
	private readonly float jumpLength = 0.30f;
	private float curJumpLength = 0;
	private bool onSteepSlope;
	private Animator animator;
	private bool canMove = true;
	bool forceApplied;
	public bool slipping;
	public Vector3 simulateMovement;

	private bool dead;

	private CameraFollow cameraFollow;

	private Vector3 inputDir;
	int s;
	float x = 0;
	float z = 0;
	
	// Use this for initialization
	void Start ()
	{
		animator = playerModel.GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
		cameraFollow = Camera.main.GetComponent<CameraFollow>();
		levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();
		initialPosition = transform.position;
		StartCoroutine (StartLevel ());
	}

	IEnumerator StartLevel(){
		canMove = false;
		yield return new WaitForSeconds (animator.runtimeAnimatorController.animationClips [0].length + 0.1f);
		canMove = true;
	}

	// Update is called once per frame
	void Update () 
	{
		s = controller.isGrounded ? 15 : 8;

		if (canMove) {
			inputDir = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			inputDir = Quaternion.Euler (0, Camera.main.transform.rotation.eulerAngles.y, 0) * inputDir;
			inputDir = inputDir.normalized;
		} else {
			inputDir = simulateMovement;
		}

		x = 0;
		z = 0;

		if (inputDir.x == 0) {
			x = Mathf.Lerp (velocity.x, 0, s * Time.deltaTime);
		} else {
			x = Mathf.Lerp (velocity.x, inputDir.x * moveSpeed, 16 * Time.deltaTime);
		}

		if (inputDir.z == 0) {
			z = Mathf.Lerp (velocity.z, 0, s * Time.deltaTime);
		} else {
			z = Mathf.Lerp (velocity.z, inputDir.z * moveSpeed, 16 * Time.deltaTime);
		}

		if (Mathf.Abs (inputDir.x) >= 0.1f || Mathf.Abs (inputDir.z) >= 0.1f) {
			g.rotation = Quaternion.Lerp (g.rotation, Quaternion.LookRotation (new Vector3 (inputDir.x, 0, inputDir.z)), 10 * Time.deltaTime);
		}
		
		if (controller.isGrounded && !Jumping) velocity.y = 0;
		velocity = new Vector3 (x, velocity.y, z);
		velocity = transform.TransformDirection (velocity);
		
		if (onSteepSlope)
			velocity += new Vector3 (slope.x, 0, slope.z).normalized * 0.25f;

		if (Jumping) {
			curJumpLength -= Time.deltaTime;
			if (curJumpLength < 0) {
				lowGravity = false;
			}
		}

		SetTriggerAnimation ("grounded", controller.isGrounded);
		
		if (controller.isGrounded || IsGroundedManual) {	
			if (!Input.GetButton ("Jump"))
				canJump = true;
			if (Input.GetButtonDown ("Jump") && canJump && canMove) {
				Jump (jumpSpeed);
			}
		}
		
		if (!controller.isGrounded) {
			onSteepSlope = false;
			slope = Vector3.up;
		}
		
		// apply gravity
		if (velocity.y > terminalVelocity)
			velocity.y -= (lowGravity && Input.GetButton ("Jump") ? gravity / 2 : gravity) * Time.deltaTime;
		
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

		transform.rotation = Quaternion.identity;
		forceApplied = false;
	}
	
	private bool IsGroundedManual {
		get {
			RaycastHit hit;
			bool didHit = Physics.Raycast (transform.position, -Vector3.up, out hit, 0.1f + controller.height / 2);
			if (didHit) {
				ColliderHit (hit.point, hit.normal, hit.collider);
			}

			SetTriggerAnimation ("grounded", didHit);
			return didHit;
		}
	}

	private void SetTriggerAnimation (string name, bool trigger)
	{
		if (trigger)
			animator.ResetTrigger ("flip_trigger");
		animator.SetBool (name, trigger);
	}

	private void OnControllerColliderHit (ControllerColliderHit hit)
	{
		ColliderHit (hit.point, hit.normal, hit.collider);
		if (velocity.y > 0 && hit.point.y >= transform.position.y + controller.height / 2)
			velocity.y = 0;
	}

	private void ColliderHit (Vector3 point, Vector3 normal, Collider collider)
	{
		if (collider.tag == "Box") {
			if (collider.GetComponent<Box> ().gameObject.activeSelf)
				collider.GetComponent<Box> ().HitPlayer (this.gameObject, point);
		} else if (collider.tag == "EndTrigger") {
			if (canMove)
				StartCoroutine (EndLevel ());
		}
		
		Rect rect = new Rect (transform.position.x - controller.radius, transform.position.z - controller.radius, controller.radius * 2, controller.radius * 2);
		if (rect.Contains (new Vector2 (point.x, point.z))) {
			slope = normal;
			float slopeAngle = Vector3.Angle (Vector3.up, normal);
			if (slopeAngle > 30 && slopeAngle < 90) {
				onSteepSlope = true;
			} else { 
				onSteepSlope = false;
			}
		}
	}

	private IEnumerator EndLevel ()
	{
		canMove = false;
		animator.SetTrigger ("end_level");
		yield return new WaitForSeconds (2f);
		levelManager.EndLevel ();
		//hit.collider.gameObject.SetActive(false);
	}

	public void Jump (float speed)
	{
		Jumping = true;
		lowGravity = true;
		curJumpLength = jumpLength;
		velocity.y = speed;
		levelManager.SoundManager.PlayClipAtPoint (PLAYER_HOP, transform.position, 0.06f);
		StartCoroutine (FlipAnim ());
	}
	
	public IEnumerator FlipAnim ()
	{	
		yield return new WaitForSeconds (0.2f);
		if ((Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) && !Spinning && !controller.isGrounded && velocity.y > 0) {
			SetTriggerAnimation ("flip_trigger", true);
			yield return new WaitForSeconds (0.3f);
		}
	}
	
	public void Spin ()
	{
		levelManager.SoundManager.PlayClipAtPoint (PLAYER_SPIN, transform.position, 0.025f);
		Spinning = true;
		
		List<Box> temp = new List<Box> ();
		temp.AddRange (touching);
		foreach (Box box in temp) {
			if (box.gameObject.activeSelf) {
				box.HitPlayer (this.gameObject, Vector3.zero);
				box.OnSpin ();
			}
		}
		
		StartCoroutine (SpinAnim ());
	}
	
	public IEnumerator SpinAnim ()
	{
		animator.SetTrigger ("spin_trigger");
		yield return new WaitForSeconds (SPIN_LENGTH);
		Spinning = false;
	}
	
	public void Die ()
	{
		//transform.position = initialPosition;
		if (!dead) levelManager.PlayerDeath ();
		dead = true;
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

	public void End ()
	{
	}

	public float CurrentRotation {
		get { return g.transform.localRotation.eulerAngles.y;}
	}

	public void ApplyVelocityInDirection (Quaternion rotation, float speed)
	{
		forceApplied = true;
		velocity += rotation * Vector3.right * speed * Time.deltaTime;
	}

	public void OnSlippingSlope(){
		if (!slipping) {
			slipping = true;
			StartCoroutine (Slip ());
		}
	}

	public void OffSlippingSlope(){
		slipping = false;
	}

	public IEnumerator Slip(){
			canMove = false;
			simulateMovement = new Vector3 (1, 0, 0);
			while (slipping) {
				yield return new WaitForSeconds (0);
			}
			yield return new WaitForSeconds (0.3f);
			canMove = true;
			simulateMovement = Vector3.zero;
	}

	public Bounds ControllerBounds { get { return controller.bounds; } }
}