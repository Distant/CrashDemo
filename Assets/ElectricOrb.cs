using UnityEngine;
using System.Collections;

public class ElectricOrb : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            CharacterControl player = col.GetComponentInParent<CharacterControl>();
            player.Die();
        }
    }
}
