using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(CharacterController))]
public class KinematicCharacterControl : MonoBehaviour, CharacterControl
{
    private static readonly string PLAYER_HOP = "player_hop";
    private static readonly string PLAYER_SPIN = "player_spin";
    private static readonly float SPIN_LENGTH = 0.5f;

    [SerializeField]
    private Transform playerModel;
    private CharacterController controller;

    private LevelManager levelManager;

    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float jumpMoveSpeed = 2;
    [SerializeField]
    private float jumpSpeed = 6f;
    [SerializeField]
    private float terminalVelocity = -8f;
    [SerializeField]
    private float gravity= 24;

    private Vector3 initialPosition;
    private Vector3 velocity = Vector3.zero;
    public Vector3 Velocity { get { return velocity; } private set { velocity = value; } }

	public bool Jumping { get; private set; }
	public float height { get; private set; }
	public bool Spinning { get; private set; }
    private bool flipping;

	public List<Box> touching = new List<Box> ();
    public Transform g;
    private Vector3 slope;

    private bool canJump;

    // Use this for initialization
    void Start ()
	{  
        controller = GetComponent<CharacterController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
        int s = controller.isGrounded ? 15 : 3;
        float x = 0;
        if (Input.GetAxis("Horizontal") == 0)
        {
            x = Mathf.Lerp(velocity.x, 0, s*Time.deltaTime);
        }
        else {
            x = Mathf.Lerp(velocity.x, Input.GetAxis("Horizontal") * moveSpeed, 8 * Time.deltaTime);
        }

        float z = 0;
        if (Input.GetAxis("Vertical") == 0)
        {
            z = Mathf.Lerp(velocity.z, 0, s*Time.deltaTime);
        }
        else
        {
            z = Mathf.Lerp(velocity.z, Input.GetAxis("Vertical") * moveSpeed, 8*Time.deltaTime);
        }

        if (controller.isGrounded && !Jumping) velocity.y = 0;
        velocity = new Vector3(x, velocity.y, z);
        velocity = transform.TransformDirection(velocity);


        if (Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) >= 0.1f)  g.rotation = Quaternion.Slerp(g.rotation, Quaternion.LookRotation(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))), 10*Time.deltaTime);

        velocity += new Vector3(slope.x, 0, slope.z).normalized * 0.2f;

        playerModel.GetComponent<Animator>().SetBool("grounded", controller.isGrounded);

        if (!Input.GetButton("Jump") && controller.isGrounded) {
            canJump = true;
        }

        if (controller.isGrounded)
        {
            if (Input.GetButton("Jump") && canJump)
            {
                Jump(jumpSpeed);
            }
        }
        else {
            slope = Vector3.up;
        }
		
		if (velocity.y > terminalVelocity)
			velocity.y -= gravity * Time.deltaTime;
        
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
            if (hit.collider.GetComponent<Box>().gameObject.activeSelf)
                hit.collider.GetComponent<Box>().HitPlayer(this.gameObject);
        } else if (hit.collider.tag == "EndTrigger") {
            levelManager.EndLevel();
            hit.collider.gameObject.SetActive(false);
        }

        Rect rect = new Rect(transform.position.x - controller.radius, transform.position.z - controller.radius, controller.radius*2, controller.radius*2);
        if (rect.Contains(new Vector2(hit.point.x, hit.point.z))){
            if (Vector3.Angle(Vector3.up, hit.normal) > 45)
            {
                print("on slope of " + hit.gameObject.name);
                slope = hit.normal;
            }
            else {
                slope = Vector3.up;
            }
        }
	}

	public void Jump (float speed)
	{
		Jumping = true;
        canJump = false;
		velocity.y = speed;
		levelManager.SoundManager.PlayClipAtPoint(PLAYER_HOP, transform.position, 0.04f);
        StartCoroutine (FlipAnim (new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"))));
	}

	public IEnumerator FlipAnim (Vector3 dir)
	{
        yield return new WaitForSeconds (0.2f);
		if ((Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) && !Spinning && !flipping) {
			flipping = true;
            playerModel.GetComponent<Animator>().SetTrigger("flip_trigger");
            flipping = false;
        } else
			yield return new WaitForSeconds (0);
	}

	public void Spin ()
	{
        levelManager.SoundManager.PlayClipAtPoint(PLAYER_SPIN, transform.position, 0.025f);
        Spinning = true;

        List<Box> temp = new List<Box>();
        temp.AddRange(touching);
        foreach (Box box in temp)
        {
            if (box.gameObject.activeSelf)
            {
                box.HitPlayer(this.gameObject);
                if (box.breakable) box.Break();
            }
		}

        StartCoroutine (SpinAnim ());
    }

	public IEnumerator SpinAnim ()
	{
        playerModel.GetComponent<Animator>().SetTrigger("spin_trigger");
        yield return new WaitForSeconds(SPIN_LENGTH);
		Spinning = false;
    }

	public void Die ()
	{
		transform.position = initialPosition;
		levelManager.PlayerDeath ();
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