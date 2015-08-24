using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{
	protected LevelManager levelManager;
	protected KinematicCharacterControl player;
	protected SoundManager soundManager;

	[SerializeField]
	private bool breakable = true;
	[SerializeField]
	private bool canSpin = true;
	[SerializeField]
	private bool canBounce = true;
	[SerializeField]
	private int bounceTotal = 0;
	[SerializeField]
	private float jumpSpeed = 6f;
	[SerializeField]
	protected float coolDownTime;

	protected float cooldown;
	protected bool onCooldown = false;
	public BoxCollider collider = null;
	private Animator animator;

	// Use this for initialization
	public virtual void Start ()
	{	
		animator = GetComponent<Animator> ();
		levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<KinematicCharacterControl> ();
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
	}

	// Update is called once per frame
	public virtual void Update ()
	{
		if (onCooldown) {
			if (cooldown >= 0) {
				cooldown -= Time.deltaTime;
			} else {
				onCooldown = false;
				if (animator != null)
					animator.SetBool ("on_cooldown", onCooldown);
			}
		}
	}

	public virtual void HitPlayer (GameObject g, Vector3 point)
	{
		//if (g.transform.position.y > transform.position.y + 0.5f && (player.Velocity.y < -2f || player.Jumping)) {
		// hit from below
		if (player.ControllerBounds.max.y <= transform.position.y - 0.24f && player.Jumping && player.Velocity.y > 0) {
			if (player.Spinning) {
				OnSpin ();
			} else {
				OnBounce (false);
			}
			// hit from above
		} else if (((player.ControllerBounds.min.y + 0.1f >= transform.position.y + 0.24f) && ((player.Velocity.y < -2f && !player.Jumping) || (player.Velocity.y < 0f && player.Jumping)))) {
			if (canBounce)
				OnBounce (true);
		}
	}

	public virtual void OnSpin ()
	{
		Break ();
	}
    
	public virtual void OnBounce (bool above)
	{
		if (player.Spinning) {
			player.Stop ();
			Break ();
		} else if (!onCooldown) {
			onCooldown = true;
			if (animator != null)
				animator.SetBool ("on_cooldown", onCooldown);
			cooldown = coolDownTime;
			player.Stop ();
			if (above)
				player.Jump (jumpSpeed);
			if (bounceTotal > 0) {
				if (animator != null)
					animator.SetTrigger (above ? "bounce" : "bounce_below");
				bounceTotal --;
			} else
				Break ();
		}
	}

	public void OnTriggerEnter (Collider col)
	{
		if (col.tag == "player_big_collider") {
			if (player.Spinning && canSpin) {
				OnSpin ();
			} else {
				print ("touching alt");
				player.Touching (this);
			}
		}
	}

	public void OnTriggerExit (Collider col)
	{
		if (col.tag == "player_big_collider") {
			CharacterControl player = col.GetComponentInParent<CharacterControl> ();
			player.NotTouching (this);
		}
	}

	public void Break ()
	{
		if (breakable) {
			soundManager.PlayClipAtPoint ("box_break", transform.position, 0.05f);
			levelManager.InventoryManager.BreakBox (5);
			gameObject.SetActive (false);
			player.NotTouching (this);
		}
	}
}