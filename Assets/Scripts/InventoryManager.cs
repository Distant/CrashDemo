using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

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
    private RectTransform inventoryPanel;
    private Vector3 hiddenPosition = new Vector3(0, 100, 0);
    private Vector3 expandedPosition = new Vector3(0, -20, 0);
    private bool expanding;

    public void init(int boxCount) {
        boxTotal = boxCount;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F)) {
            StartCoroutine(ExpandInventory());
        }
	
	}

    public IEnumerator ExpandInventory() {
        while (true) {
            inventoryPanel.anchoredPosition = inventoryPanel.anchoredPosition - new Vector2(0, 4f);
            if (inventoryPanel.anchoredPosition.y < expandedPosition.y + 1f) {
                inventoryPanel.anchoredPosition = expandedPosition;
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            inventoryPanel.anchoredPosition = inventoryPanel.anchoredPosition + new Vector2(0, 4f);
            if (inventoryPanel.anchoredPosition.y > hiddenPosition.y - 1f)
            {
                inventoryPanel.anchoredPosition = hiddenPosition;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

	public int Wumpas(){
		return wumpas;
	}

	public void BreakBox(int w){
		wumpas += w;
		if (wumpas > 49) {
			wumpas = wumpas - 50;
		}
		if (!counting) {
			counting = true;
			StartCoroutine (UpdateWumpaText ());
		}
		boxCount ++;
		boxText.text = boxCount.ToString ();
	}

	public IEnumerator UpdateWumpaText() {
		while(tempwumpas < wumpas || (tempwumpas > wumpas && tempwumpas < 50)) {
			tempwumpas += 1;
			if (tempwumpas == 49){
				addLives(1);
				tempwumpas = 0;
			}
			wumpaText.text = tempwumpas.ToString ();
			yield return new WaitForSeconds(0.2f);
		}
		counting = false;
		yield return new WaitForSeconds(0);
	}

	public void addLives(int l){
		lives += l;
		if (lives > 99) {
			lives = 99;
		}
		else if (lives < 0) {
			lives = 0;
		}
		lifeText.text = lives.ToString ();
	}

	public int getLives(){
		return lives;
	}

	public void death(){
		wumpas = 0;
		tempwumpas = 0;
		addLives (-1);
		boxCount = 0;
		wumpaText.text = tempwumpas.ToString ();
		lifeText.text = lives.ToString ();
		boxText.text = boxCount.ToString ();
	}

	public void VerifyBoxCount(){
		string finalText;
		if (boxCount == boxTotal)
			finalText = "you got all the boxes!";
		else
			finalText = "you missed " + (boxTotal - boxCount) + " boxes :(";
		print (finalText);
	}
}
