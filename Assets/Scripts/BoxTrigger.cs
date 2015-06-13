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
            StartCoroutine(PlayJump());
        }
    }

    public override void OnSpin()
    {
        if (repeatable ? !onCooldown : !activated)
        {
            base.OnBounce();
            Trigger();
            StartCoroutine(PlaySpin());
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

    public IEnumerator PlayJump()
    {
        onCooldown = true;
        float elapsed = 0;
        while (true)
        {
            if (elapsed > 0.05f) break;
            mesh.localScale = Vector3.Lerp(mesh.localScale, scaleSmall, 30 * Time.deltaTime);
            mesh.position = Vector3.Lerp(mesh.position, initPos - new Vector3(0, smallY, 0), 20 * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        elapsed = 0;
        while (true)
        {

            if (elapsed > 0.1f) break;
            mesh.localScale = Vector3.Lerp(mesh.localScale, scaleBig, 30 * Time.deltaTime);
            mesh.position = Vector3.Lerp(mesh.position, initPos + new Vector3(0, bigY, 0), 10 * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        elapsed = 0;
        while (true)
        {
            if (elapsed > 0.05f) break;
            mesh.localScale = Vector3.Lerp(mesh.localScale, initScale, 20 * Time.deltaTime);
            mesh.position = Vector3.Lerp(mesh.position, initPos, 20 * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        mesh.localScale = initScale;
        mesh.position = initPos;


        yield return new WaitForSeconds(0.2f);
        onCooldown = false;
    }

    public IEnumerator PlaySpin()
    {
        onCooldown = true;
        float elapsed = 0;
        while (true)
        {
            if (elapsed > 0.1f) break;
            mesh.localScale = Vector3.Lerp(mesh.localScale, scaleBigSpin, 30 * Time.deltaTime);
            mesh.position = Vector3.Lerp(mesh.position, initPos - new Vector3(0, bigYSpin, 0), 20 * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        elapsed = 0;
        while (true)
        {
            if (elapsed > 0.1f) break;
            mesh.localScale = Vector3.Lerp(mesh.localScale, initScale, 20 * Time.deltaTime);
            mesh.position = Vector3.Lerp(mesh.position, initPos, 20 * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        mesh.localScale = initScale;
        mesh.position = initPos;


        yield return new WaitForSeconds(0.2f);
        onCooldown = false;
    }
}