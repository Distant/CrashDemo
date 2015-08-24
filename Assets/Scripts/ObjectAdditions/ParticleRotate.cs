using UnityEngine;
using System.Collections;

public class ParticleRotate : MonoBehaviour
{

    private GameObject player;
    bool playerNear;
    public Light light2;
    private bool lightFadingIn;

    public Light[] lights;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        this.GetComponent<Animator>().Play("rotate", 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerNear)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 6f)
            {
                GetComponent<ParticleSystem>().Play();
                StartCoroutine(FadeInLight());
                playerNear = true;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 6f)
            {
                GetComponent<ParticleSystem>().Stop();
                playerNear = false;
                StartCoroutine(FadeOutLight());
            }
        }
    }

    public IEnumerator FadeInLight()
    {
        lightFadingIn = true;
        while (lightFadingIn)
        {
            foreach (Light light in lights)
                light.intensity = Mathf.Lerp(light.intensity, 3, 1.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator FadeOutLight()
    {
        lightFadingIn = false;
        while (!lightFadingIn)
        {
            foreach (Light light in lights)
                light.intensity = Mathf.Lerp(light.intensity, 0, 1.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
