using UnityEngine;
using System.Collections;

public class ElectricPoles : MonoBehaviour {

    public Transform o1;
    public Transform o2;
    public Vector3 start1;
    public Vector3 start2;
    public float end1X;
    public float end2X;

    // Use this for initialization
    void Start () {
        start1 = o1.localPosition;
        start2 = o2.localPosition;
        StartCoroutine(Animate1());
        StartCoroutine(RotateAnim());
    }
	
	// Update is called once per frame
	void Update () {

	}

    public IEnumerator Animate1() {
        float time = 0;
        while (true) {
            while (o1.localPosition.x < start1.x + end1X - 8 * Time.deltaTime)
            {
                o1.localPosition += new Vector3(8,0,0) * Time.deltaTime;
                o2.localPosition -= new Vector3(7.2f, 0, 0) * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            o1.localPosition = start1 + new Vector3(end1X, 0, 0);
            o2.localPosition = start2 + new Vector3(end2X, 0, 0);

            time = 0;

            while( time < 0.1f)
            {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            while (o1.localPosition.x > start1.x + 8 * Time.deltaTime)
            {
                o1.localPosition -= new Vector3(8, 0, 0) * Time.deltaTime;
                o2.localPosition += new Vector3(7.2f, 0, 0) * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            o1.localPosition = start1;
            o2.localPosition = start2;

            time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void rotate(Transform t) {
        t.Rotate(Vector3.up, 1500 * Time.deltaTime);
        t.Rotate(Vector3.left, 1500 * Time.deltaTime);
        t.Rotate(Vector3.forward, 1500 * Time.deltaTime);
    }

    public IEnumerator RotateAnim() {
        while (true) {
            rotate(o1);
            rotate(o2);
            yield return new WaitForEndOfFrame();
        }
    }
}
