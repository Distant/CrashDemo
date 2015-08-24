using UnityEngine;
using System.Collections;

public class AnimationOffset : MonoBehaviour {

    public float animOffset;

	[SerializeField]
	private string animName;
	[SerializeField]
	private int layer;

	// Use this for initialization
	void Start () {
        this.GetComponent<Animator>().Play(animName, layer, animOffset);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
