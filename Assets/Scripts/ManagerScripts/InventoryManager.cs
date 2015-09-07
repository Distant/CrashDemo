using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

	private int wumpas;
	public int Wumpas { get { return wumpas; } }

	private int lives = 5;
	public int getLives { get { return lives; } }

	private bool counting;
	private int boxCount;
	private int boxTotal;
	private SoundManager soundManager;
	[SerializeField] private ItemInventoryUI itemUI;

	public void init (int boxCount)
	{
		boxTotal = boxCount;
	}

	// Use this for initialization
	void Start ()
	{

		itemUI.LifeCount = lives;
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.F)) {
			itemUI.ToggleInventory();
		}

	}

	public void AddWumpa (Transform t)
	{	
		IncrementWumpas (1);
		StartCoroutine (itemUI.UpdateWumpaText (Camera.main.WorldToScreenPoint(t.position)));
	}

	public void AddWumpas (int w, Transform t)
	{
		IncrementWumpas (w);
		StartCoroutine (itemUI.UpdateWumpText (w, t));
		boxCount++;
		itemUI.BoxCount = boxCount;
	}

	private void IncrementWumpas (int w)
	{
		wumpas += w;
		if (wumpas > 99) {
			wumpas = wumpas - 100;
			addLives (1);
		}
	}

	public void addLives (int l)
	{
		lives += l;
		if (lives > 99) {
			lives = 99;
		} else if (lives < 0) {
			lives = 0;
		}
		itemUI.LifeCount = lives;
	}
	public void Death ()
	{
		wumpas = 0;
		addLives (-1);
		boxCount = 0;
		itemUI.WumpaCount = 0;
		itemUI.BoxCount = boxCount;
		itemUI.LifeCount = lives;
		itemUI.Reset ();
	}

	public void VerifyBoxCount ()
	{
		string finalText;
		if (boxCount == boxTotal)
			finalText = "you got all the boxes!";
		else
			finalText = "you missed " + (boxTotal - boxCount) + " boxes :(";
		print (finalText);
	}

	public void ShowItem (PickupType type)
	{
		itemUI.ShowItem (type);
	}

	public void Destroy(){
		Destroy (itemUI.transform.parent.gameObject);
	}
}
