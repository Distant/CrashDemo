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
	[SerializeField] private int wumpaCount = 5;
	[SerializeField] private bool autoWumpas;
	public BoxCollider BoxCollider = null;
	protected Animator animator;
	private ParticleSystem particles;
	protected float cooldown;
	protected bool onCooldown = false;
	public Transform wumpaObject;

	// Use this for initialization
	public virtual void Start ()
	{	
		animator = GetComponent<Animator> ();
		if (animator != null)
			animator.logWarnings = false;
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
		if (PlayerBelow (g, point)) {
			if (player.Spinning) {
				OnSpin ();
			} else {
				OnBounce (false);
			}

		} else if (PlayerAbove (g, point)) {
			if (player.Spinning) {
				player.Stop ();
				OnSpin ();
			} else if (canBounce) {
				OnBounce (true);
			}
		}
	}

	private bool PlayerAbove (GameObject g, Vector3 point)
	{
		return (player.ControllerBounds.min.y + 0.1f >= transform.position.y + 0.24f) && ((player.Velocity.y < -2f && !player.Jumping) || (player.Velocity.y < 0f && player.Jumping));
	}

	private bool PlayerBelow (GameObject g, Vector3 point)
	{
		return player.ControllerBounds.max.y <= transform.position.y - 0.24f && player.Jumping && player.Velocity.y > 0;
	}

	public virtual void OnSpin ()
	{
		Break ();
	}

	public virtual void Trigger ()
	{
		if (!onCooldown) {
			onCooldown = true;
			if (animator != null)
				animator.SetBool ("on_cooldown", onCooldown);
			soundManager.PlayClipAtPoint ("bouncy_box_bounce", transform.position, 0.05f);
			cooldown = coolDownTime;
		}
	}
    
	public virtual void BouncePlayer (bool above)
	{
		player.Stop ();
		if (above) {
			player.Jump (jumpSpeed);
		}
		if (bounceTotal > 0) {
			if (animator != null)
				animator.SetTrigger (above ? "bounce" : "bounce_below");
			SingleBounce ();
		} else
			Break ();
	}

	public virtual void OnBounce (bool above)
	{
		Trigger ();
		BouncePlayer (above);
	}

	protected virtual void SingleBounce ()
	{
		levelManager.InventoryManager.AddWumpa (transform);
		bounceTotal --;
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
			soundManager.PlayClipAtPoint ("box_break", transform.position, 0.08f);
			player.NotTouching (this);
			if (autoWumpas)
				levelManager.InventoryManager.AddWumpas (wumpaCount, transform); 
			else {
				for (int i = 0; i < wumpaCount; i++) {
					Instantiate (wumpaObject).position = wumpaCount == 1 ? transform.position : transform.position +  new Vector3 (Random.Range (-0.3f, 0.3f), Random.Range (-0.075f, 0.075f), Random.Range(-0.1f, 0.1f));
				}
			}
			//levelManager.InventoryManager.AddWumpas (wumpaCount,Camera.main.WorldToScreenPoint(transform.position));
			StartCoroutine (DestroyObject ());
		}
	}

	public IEnumerator DestroyObject ()
	{
		foreach (BoxCollider col in GetComponents<BoxCollider> ()) {
			col.enabled = false;
		}
		GetComponentInChildren<MeshRenderer> ().enabled = false;
		particles.Play ();
		yield return new WaitForSeconds (2f);
		Destroy (gameObject);
	}
}