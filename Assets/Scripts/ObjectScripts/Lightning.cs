using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

	float amountOfOff = 0.9f;
	float amountOfOn = 0.015f; 
	Light l;

	void Start(){
		l = GetComponent<Light> ();
	}

	void Update()
	{
		if(l.enabled && (Random.value > amountOfOff))
			l.enabled = false;
		else
			if(Random.value < amountOfOn)
				l.enabled = true;
	}
	

}
