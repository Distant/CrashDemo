using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BoxTrigger : Box
{

    public float cooldown = 0.5f;
    private bool onCooldown = false;
    public Vector3 scaleBig;
    public Vector3 scaleBigSpin;
    public Vector3 scaleSmall;
    private Vector3 initScale;
    private Vector3 initPos;
    public float smallY;
    public float bigY;
    public float bigYSpin;
    public bool repeatable;
    private bool activated;

    public Transform mesh;

    public UnityEvent trigger;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        initScale = mesh.localScale;
        initPos = mesh.position;
    }

    public override void OnBounce()
    {
        if (repeatable ? !onCooldown : !activated)
        {
            base.OnBounce();
            Trigger();
            mesh.GetComponent<Animator>().Play("box_bounce");
        }
    }

    public override void OnSpin()
    {
        if (repeatable ? !onCooldown : !activated)
        {
            base.OnBounce();
            Trigger();
            mesh.GetComponent<Animator>().Play("box_spin");
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