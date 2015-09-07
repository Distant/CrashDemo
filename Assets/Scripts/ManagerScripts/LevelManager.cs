using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public GameObject BoxesObj;
    private Box[] boxes;

    private GameManager game;

    [SerializeField]
    private InventoryManager inventoryManager;
    public InventoryManager InventoryManager { get { return inventoryManager; } private set { inventoryManager = value; } }

    [SerializeField]
    private SoundManager soundManager;
    public SoundManager SoundManager { get { return soundManager; } private set { soundManager = value; } }

    private static bool created = false;

    [SerializeField]
    private int bonusId;
    public int BonusId { get { return bonusId; } }

    void Awake()
    {
        if (created) DestroyImmediate(this.gameObject);
        FindGameManager(out game);
    }

    // Use this for initialization
    void Start()
    {
        created = true;
        DontDestroyOnLoad(this.gameObject);

        boxes = BoxesObj.GetComponentsInChildren<Box>();
        inventoryManager.init(boxes.Length);
    }

    private void FindGameManager(out GameManager game)
    {
        GameObject g = GameObject.FindGameObjectWithTag("GameManager");
        if (g != null) game = g.GetComponent<GameManager>();
        else game = null;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerDeath()
    {
        inventoryManager.Death();
        Application.LoadLevel(Application.loadedLevel);
        soundManager.Reset();
    }

    public void EndLevel()
    {
		CleanUp ();
		if (game != null) {
			game.EndLevel(Application.loadedLevel);
		}
    }

	public void CleanUp(){
		inventoryManager.VerifyBoxCount ();
		created = false;
		inventoryManager.Destroy ();
		Destroy (this.gameObject);
	}

    public void CollectItem(PickupType type)
    {
        if (game != null)
        {
            game.AddItem(type);
        }
        inventoryManager.ShowItem(type);
    }

    public bool CheckGlobalItem(PickupType type) {
        if (game != null) {
            return game.CheckItem(type);
        }
        return false;
    }
}