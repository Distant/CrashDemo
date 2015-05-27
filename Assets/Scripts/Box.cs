using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{
	private InventoryManager inventoryManager;

	// Use this for initialization
	void Start ()
	{
		inventoryManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<InventoryManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void OnCollisionEnter (Collision col)
	{
		if (col.contacts.Length > 0) {
			ContactPoint[] c = col.contacts;
			if (Vector3.Dot (c [0].normal, Vector3.down) > 0.5f) {
				inventoryManager.addWumpas(5);
				gameObject.SetActive(false);
			}
		}
	}
}
