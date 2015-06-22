using UnityEngine;
using System.Collections;

public class ElectricPoles : MonoBehaviour {

    public Transform o1;
    public Transform o2;
    public Vector3 start1;
    public Vector3 start2;
    public float end1X;
    public float end2X;
    private SoundManager sound;

    // Use this for initialization
    void Start () {
        start1 = o1.localPosition;
        start2 = o2.localPosition;
        //StartCoroutine(RotateAnim());
        sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void rotate(Transform t) {
        t.Rotate(Vector3.up, Random.Range(0,1000) * Time.deltaTime);
        t.Rotate(Vector3.left, Random.Range(0, 1000) * Time.deltaTime);
        t.Rotate(Vector3.forward, Random.Range(0, 1000) * Time.deltaTime);
    }

    public IEnumerator RotateAnim() {
        while (true) {
            rotate(o1);
            rotate(o2);
            yield return new WaitForEndOfFrame();
        }
    }

    public void PlaySound() {
        sound.PlayClipAtPoint("orb_zap", transform.position, 0.2f);
    }
}
