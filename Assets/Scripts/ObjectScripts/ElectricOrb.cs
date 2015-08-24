using UnityEngine;
using System.Collections;

public class ElectricOrb : MonoBehaviour {

    public Texture[] textures;
    private Renderer renderer2;
    private int index;
    private int temp;

	// Use this for initialization
	void Start () {
        renderer2 = GetComponent<Renderer>();
        StartCoroutine(changeTexture());
    }
	
	// Update is called once per frame
	void Update () {
    }

    public IEnumerator changeTexture() {
        while (true) {
            while (temp == index) {
                temp = (int)Random.Range(0, 4);
            }
            renderer2.material.SetTexture("_EmissionMap", textures[index = temp]);
            yield return new WaitForSeconds(0.05f);
        }
    }
    
}
