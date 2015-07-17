using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BoxTrigger : Box
{

    public float cooldown;
    private bool onCooldown = false;
    public Vector3 scaleBig;
    public Vector3 scaleBigSpin;
    public Vector3 scaleSmall;
    public float smallY;
    public float bigY;
    public float bigYSpin;
    public bool repeatable;
    private bool activated;

    public Transform mesh;

    public UnityEvent trigger;

    private Animator animator;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public void Update() {
        if (cooldown >= 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            onCooldown = false;
            animator.SetBool("on_cooldown", onCooldown);
        }
    }

    public override void OnBounce()
    {
        if (repeatable ? !onCooldown : !activated)
        {
            base.OnBounce();
            Trigger();
            animator.SetTrigger("bounce");
            levelManager.SoundManager.PlayClipAtPoint("box_trigger", transform.position, 0.05f);
            onCooldown = true;
            animator.SetBool("on_cooldown", onCooldown);
            cooldown = 0.4f;
        }
    }

    public override void OnSpin()
    {
        if (repeatable ? !onCooldown : !activated)
        {
            base.OnBounce();
            Trigger();
            animator.SetTrigger("spin");
            levelManager.SoundManager.PlayClipAtPoint("box_trigger", transform.position, 0.05f);
            onCooldown = true;
            animator.SetBool("on_cooldown", onCooldown);
            cooldown = 0.4f;
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