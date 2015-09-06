using UnityEngine;
using System.Collections;

public class BoxInvisible : Box
{

	// Use this for initialization
	public override void Start ()
	{
        base.Start();
    } 

	public override void HitPlayer(GameObject g, Vector3 point) {
        Enable();
        base.HitPlayer(g, point);
	}

    public void Enable() {
        GetComponent<MeshRenderer>().enabled = true;
    }

}