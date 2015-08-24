using UnityEngine;
using System.Collections;

public class GemPhysical : MonoBehaviour {

	public Vector3 endPos;
    private Vector3 startPos;
	private bool moving;
    
	private int position;
	private float speed = 1f ;

	[SerializeField]
    private PickupType type;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        if (!GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().CheckGlobalItem(type)){
            //SetDisabled();
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnTriggerEnter(Collider col)
    {
		if (!moving) {
			if (position == 0) {
				StartCoroutine (Move (startPos, endPos));
				position = 1;
			} else { 
				StartCoroutine (Move (endPos, startPos));
				position = 0;
			}
		}
    }


    public IEnumerator Move(Vector3 start, Vector3 end) {
        moving = true;
        yield return new WaitForSeconds(0.35f);
        Vector3 dif = (end - start) * 0.5f;
        while (Vector3.Distance(transform.position, end) > 0.1f) {
            transform.position += dif.normalized * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        moving = false;
    }

    private void SetDisabled() {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        transform.localScale *= 0.3f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(endPos, 0.2f);
    }
}