using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

	private int wumpas;
	private int tempwumpas;
	private int lives = 5;
	public Text wumpaText;
	public Text boxText;
	public Text lifeText;
	private bool counting;
	private int boxCount;
	private int boxTotal;
	[SerializeField]
	private ItemInventoryUI itemUI;
	[SerializeField]
	public RectTransform inventoryPanel;
	private Vector3 hiddenPosition = new Vector3 (0, 0, 0);
	private Vector3 expandedPosition = new Vector3 (0, -50, 0);
	private bool expanding;
	private bool isOpen = false;
	SoundManager soundManager;
	public Image test_wumpa;

	public void init (int boxCount)
	{
		boxTotal = boxCount;
	}

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (inventoryPanel.parent.gameObject);
		lifeText.text = lives.ToString ();
		StartCoroutine (ExpandInventory ());

		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.F)) {
			if (!isOpen)
				StartCoroutine (ExpandInventory ());
			else
				StartCoroutine (HideInventory ());
		}

	}

	public IEnumerator ExpandInventory ()
	{
		isOpen = true;
		while (isOpen) {
			inventoryPanel.anchoredPosition = inventoryPanel.anchoredPosition - new Vector2 (0, 4f);
			if (inventoryPanel.anchoredPosition.y < expandedPosition.y + 1f) {
				inventoryPanel.anchoredPosition = expandedPosition;
				break;
			}
			yield return new WaitForEndOfFrame ();
		}
	}

	public IEnumerator HideInventory ()
	{
		isOpen = false;
		while (!isOpen) {
			inventoryPanel.anchoredPosition = inventoryPanel.anchoredPosition + new Vector2 (0, 4f);
			if (inventoryPanel.anchoredPosition.y > hiddenPosition.y - 1f) {
				inventoryPanel.anchoredPosition = hiddenPosition;
				break;
			}
			yield return new WaitForEndOfFrame ();
		}
	}

	public int Wumpas ()
	{
		return wumpas;
	}

	public void AddWumpa (Transform t)
	{	
		IncrementWumpas (1);
		StartCoroutine (UpdateWumpaText (Camera.main.WorldToScreenPoint(t.position)));
	}

	public void AddWumpas (int w, Transform t)
	{
		IncrementWumpas (w);
		StartCoroutine (UpdateWumpTextAuto (w, t));
		boxCount++;
		boxText.text = boxCount.ToString ();
	}

	private void IncrementWumpas (int w)
	{
		wumpas += w;
		if (wumpas > 99) {
			wumpas = wumpas - 100;
			addLives (1);
		}
	}

	private IEnumerator AnimateWumpa (Image i, Vector3 pos)
	{
		i.rectTransform.SetParent (inventoryPanel.parent, false);
		i.rectTransform.position = pos;
		Vector3 target = new Vector3 (38, Camera.main.pixelHeight - 28, 0);
		while (true) {
			i.rectTransform.position = Vector3.MoveTowards (i.rectTransform.position, target, 25f); 
			if (Vector3.Distance (i.rectTransform.position, target) < 0.1f) {
				Destroy (i.gameObject);
				break;
			}
			yield return new WaitForSeconds (0);
		}
	}

	private IEnumerator UpdateWumpaText (Vector3 pos)
	{
		StartCoroutine (AnimateWumpa (Instantiate (test_wumpa), pos));
		soundManager.PlayClipAtPoint ("wumpa_collect", transform.position, 0.01f);
		yield return new WaitForSeconds (0.55f);
		tempwumpas += 1;
		if (tempwumpas == 100) {
			tempwumpas = 0;
		}
		wumpaText.text = tempwumpas.ToString ();
	}

	private IEnumerator UpdateWumpTextAuto (int w, Transform t)
	{
		for (int i =0; i < w; i++) 
		{
			if (t == null) break;
			StartCoroutine (UpdateWumpaText (Camera.main.WorldToScreenPoint(t.position)));
			yield return new WaitForSeconds (0.2f);
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
		lifeText.text = lives.ToString ();
	}

	public int getLives ()
	{
		return lives;
	}

	public void Death ()
	{
		wumpas = 0;
		tempwumpas = 0;
		addLives (-1);
		boxCount = 0;
		wumpaText.text = tempwumpas.ToString ();
		lifeText.text = lives.ToString ();
		boxText.text = boxCount.ToString ();
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
}
