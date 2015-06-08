using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public GameObject BoxesObj;
	private Box[] boxes;
    public Text image;

    [SerializeField]
	private InventoryManager inventoryManager;
    public InventoryManager InventoryManager { get { return inventoryManager; } private set { inventoryManager = value; } }

    [SerializeField]
    private SoundManager soundManager;
    public SoundManager SoundManager { get { return soundManager; } private set { soundManager = value; } }

	// Use this for initialization
	void Start () {
        image.enabled = false;
		boxes = BoxesObj.GetComponentsInChildren<Box>();
        inventoryManager.init(boxes.Length);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayerDeath(){
		inventoryManager.death ();
		foreach (Box box in boxes) {
			box.gameObject.SetActive(true);
		}
	}

	public void EndLevel(){
		inventoryManager.VerifyBoxCount ();
        image.enabled = true;
        Application.LoadLevel(1);
    }
}