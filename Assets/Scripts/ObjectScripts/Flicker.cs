using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour {

    public float minFlickerSpeed = 0.05f;
    public float maxFlickerSpeed = 0.2f;
    public float maxEm = 1.02f;
    public float minEm = 0.97f;
    public Material material;

	// Use this for initialization
	void Start () {
        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;
        StartCoroutine(FlickerAnim());
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public IEnumerator FlickerAnim() {
        Color col = material.GetColor("_EmissionColor");
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
            material.SetColor("_EmissionColor", col * minEm);
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
            material.SetColor("_EmissionColor", col * maxEm);
        }
    }
}
