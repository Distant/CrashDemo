using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BoxTrigger : Box {

    public float cooldown = 0.5f;
    public bool cooling = false;

    public UnityEvent trigger;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    public override void OnBounce() {
        base.OnBounce();
        Trigger();
    }

    public override void OnSpin() {
        base.OnBounce();
        Trigger();
    }

    public void Trigger() {
        if (!cooling) {
            trigger.Invoke();
            StartCoroutine(Play());
        }
    }

    public IEnumerator Play() {
        cooling = true;
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(cooldown);
        cooling = false;
    }
}