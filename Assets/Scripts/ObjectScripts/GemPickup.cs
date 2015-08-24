using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GemPickup : MonoBehaviour {

    public PickupType type;
    private LevelManager levelManager;
	// Use this for initialization
	void Start () {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        if (levelManager.CheckGlobalItem(type)) {Destroy(this.gameObject); }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            levelManager.SoundManager.PlayClipAtPoint("box_trigger", transform.position, 0.2f);
            levelManager.CollectItem(type);
            Destroy(this.gameObject);
        }
    }
}
