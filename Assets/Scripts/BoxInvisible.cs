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
        GetComponent<MeshRenderer>().enabled = true;
        base.HitPlayer(g);
	}
}