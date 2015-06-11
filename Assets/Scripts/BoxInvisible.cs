using UnityEngine;
using System.Collections;

public class BoxInvisible : Box
{

	// Use this for initialization
	public override void Start ()
	{
        base.Start();
    }
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public override void HitPlayer(GameObject g) {
        Enable();
        base.HitPlayer(g);
	}

    public void Enable() {
        GetComponent<MeshRenderer>().enabled = true;
    }

}