using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour {

	private Light spotLight;

	[SerializeField]
	private Text text;

	[SerializeField]
	private TextMesh textMesh;

	[SerializeField]
	private Material boxMaterial;
	private Color initColor;

	void Awake(){
		spotLight = GetComponentInChildren<Light> ();
		Deselect ();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () { 
	
	}

	public void DisableNode(){
		//GetComponent<MeshRenderer>().material.color = new Color(0.5f,0.5f,0.5f,1);
		gameObject.SetActive (false);
	}

	public void Select(){
		spotLight.enabled = true;
		//text.GetComponent<Gradient> ().enabled = false;
		textMesh.color = new Color(1,1,1,1);
	}

	public void Deselect(){
		spotLight.enabled = false;
		//text.GetComponent<Gradient> ().enabled = true;
		textMesh.color = new Color(0.1f,0.1f,0.1f,1);
	}
}
