using UnityEngine;
using System.Collections;

public class EndLevelTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") {
            col.SendMessage("End");
        }
        
    }
    
}
