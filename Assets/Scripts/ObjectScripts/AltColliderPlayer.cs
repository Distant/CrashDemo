using UnityEngine;
using System.Collections;

public class AltColliderPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void OnColliderEnter(Collision col) {
        if (col.transform.tag == "Box")
        {
            if (col.transform.GetComponent<Box>().gameObject.activeSelf)
                col.transform.GetComponent<Box>().HitPlayer(this.gameObject, col.contacts[0].point);
        }
    }   
}
