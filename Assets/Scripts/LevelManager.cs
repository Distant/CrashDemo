using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public GameObject BoxesObj;
    private Box[] boxes;
    public Text image;

    [SerializeField]
    private InventoryManager inventoryManager;
    public InventoryManager InventoryManager { get { return inventoryManager; } private set { inventoryManager = value; } }

    [SerializeField]
    private SoundManager soundManager;
    public SoundManager SoundManager { get { return soundManager; } private set { soundManager = value; } }

    private static bool created = false;

    void Awake()
    {
        if (created) DestroyImmediate(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        created = true;
        DontDestroyOnLoad(this.gameObject);
        image.enabled = false;
        boxes = BoxesObj.GetComponentsInChildren<Box>();
        inventoryManager.init(boxes.Length);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerDeath()
    {
        inventoryManager.Death();
        soundManager.Reset();
        Application.LoadLevel(Application.loadedLevel);
    }

    public void EndLevel()
    {
        inventoryManager.VerifyBoxCount();
        image.enabled = true;
        created = false;
        Application.LoadLevel(1);
        Destroy(inventoryManager.inventoryPanel.parent.gameObject);
        Destroy(this.gameObject);
    }
}