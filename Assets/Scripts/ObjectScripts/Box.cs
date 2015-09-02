using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour 
{
	protected LevelManager levelManager;
	protected CharacterControl player;
	protected SoundManager soundManager;

	[SerializeField] private bool breakable = true;
	[SerializeField] private bool canSpin = true;
	[SerializeField] private bool canBounce = true;
	[SerializeField] private int bounceTotal = 0;
	[SerializeField] private float jumpSpeed = 6f;
	[SerializeField] protected float coolDownTime;

	public BoxCollider collider = null;
	protected Animator animator;
	private ParticleSystem particles;
	protected float cooldown;
	protected bool onCooldown = false;

	// Use this for initialization
	public virtual void Start ()
	{	
		animator = GetComponent<Animator> ();
		particles = GetComponent<ParticleSystem> ();
		levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterControl> ();
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
			if (player.Spinning) {
				player.Stop () ;
				OnSpin ();
			}
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
		 if (!onCooldown) {
			onCooldown = true;
			if (animator != null) animator.SetBool ("on_cooldown", onCooldown);
			soundManager.PlayClipAtPoint ("bouncy_box_bounce", transform.position, 0.07f);
			cooldown = coolDownTime;
			player.Stop ();
			if (above)
				player.Jump (jumpSpeed);
			if (bounceTotal > 0) {
				if (animator != null) animator.SetTrigger (above ? "bounce" : "bounce_below");
				bounceTotal --;
				//soundManager.PlayClipAtPoint ("wumpa_collect", transform.position, 0.03f);
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
			player.NotTouching (this);
			//soundManager.PlayClipAtPoint ("wumpa_collect", transform.position, 0.03f);
			StartCoroutine(DestroyObject());
		}
	}

	public IEnumerator DestroyObject(){
		foreach (BoxCollider col in GetComponents<BoxCollider> ()) {
			col.enabled = false;
		}
		GetComponentInChildren<MeshRenderer> ().enabled = false;
		particles.Play();
		yield return new WaitForSeconds (2f);
		Destroy (gameObject);
	}
}