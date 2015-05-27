using UnityEngine;
using System.Collections;

public class NodeControl : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void OnTriggerEnter (Collider other)
	{
		if (other.tag == "CameraNodeTrigger") {
			CameraFollow cameraFollow = ((CameraFollow)Camera.main.gameObject.GetComponent<CameraFollow> ());
			cameraFollow.NextNode (other.gameObject.GetComponentInParent<CameraNode> ());
		}
	}
}