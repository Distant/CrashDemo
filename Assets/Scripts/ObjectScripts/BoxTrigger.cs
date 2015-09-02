using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BoxTrigger : Box
{
	[SerializeField]
    private bool repeatable;
    private bool activated;

    public Transform mesh;

    public UnityEvent trigger;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public void Update() {
		base.Update ();
    }

    public override void OnBounce(bool above)
    {
        if (repeatable ? !onCooldown : !activated)
        {
            base.OnBounce(above);
            Trigger();
            animator.SetTrigger("bounce");
            levelManager.SoundManager.PlayClipAtPoint("box_trigger", transform.position, 0.05f);
        }
    }

    public override void OnSpin()
    {
        if (repeatable ? !onCooldown : !activated)
        {
            base.OnBounce(true);
            Trigger();
            animator.SetTrigger("spin");
            levelManager.SoundManager.PlayClipAtPoint("box_trigger", transform.position, 0.05f);
            onCooldown = true;
            animator.SetBool("on_cooldown", onCooldown);
			cooldown = coolDownTime;
        }
    }

    public void Trigger()
    {
        if (!activated && !repeatable)
        {
            activated = true;
            mesh.gameObject.GetComponent<MeshRenderer>().material = mesh.gameObject.GetComponent<MeshRenderer>().materials[1];
        }
        trigger.Invoke();
    }
}