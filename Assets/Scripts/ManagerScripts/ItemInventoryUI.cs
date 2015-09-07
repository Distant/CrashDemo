using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemInventoryUI : MonoBehaviour
{

    [SerializeField]
    private PickupType[] itemTypes;
    [SerializeField]
    private Image[] images;

    private Transform player;
    private Transform mainCamera;

    private Dictionary<PickupType, Image> ItemImages;
	private SoundManager soundManager;
	public RectTransform inventoryPanel;
	
	private Vector3 hiddenPosition = new Vector3 (0, 0, 0);
	private Vector3 expandedPosition = new Vector3 (0, -50, 0);
	private bool expanding;
	private bool isOpen = false;

	private int tempwumpas;

	public Text wumpaText;
	public int WumpaCount { set { wumpaText.text = value.ToString (); } }

	public Text boxText;
	public int BoxCount { set { boxText.text = value.ToString (); } }

	public Text lifeText;
	public int LifeCount { set { lifeText.text = value.ToString (); } }

	[SerializeField] Image test_wumpa;

    // Use this for initialization
    void Start()
    {
		DontDestroyOnLoad (inventoryPanel.parent.gameObject);
        ItemImages = new Dictionary<PickupType, Image>();
        for (int i = 0; i < Mathf.Min(itemTypes.Length, images.Length); i++)
        {
            ItemImages.Add(itemTypes[i], images[i]);
        }
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();

		StartCoroutine (ExpandInventory ());
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void ToggleInventory(){
		if (Input.GetKeyDown (KeyCode.F)) {
			if (!isOpen)
				StartCoroutine (ExpandInventory ());
			else
				StartCoroutine (HideInventory ());
		}
	}

	public IEnumerator UpdateWumpaText (Vector3 pos)
	{
		StartCoroutine (AnimateWumpa (Instantiate (test_wumpa), pos));
		yield return new WaitForSeconds (0);
	}
	
	public IEnumerator UpdateWumpText (int w, Transform t)
	{
		for (int i =0; i < w; i++) 
		{
			if (t == null) break;
			StartCoroutine (UpdateWumpaText (Camera.main.WorldToScreenPoint(t.position)));
			yield return new WaitForSeconds (0.2f);
		}
	}
	
	private IEnumerator AnimateWumpa (Image i, Vector3 pos)
	{
		soundManager.PlayClipAtPoint ("wumpa_pickup", transform.position, 0.02f);
		i.rectTransform.SetParent (inventoryPanel.parent, false);
		i.rectTransform.position = pos;
		Vector3 target = new Vector3 (38, Camera.main.pixelHeight - 28, 0);
		while (true) {
			i.rectTransform.position = Vector3.MoveTowards (i.rectTransform.position, target, 40f); 
			if (Vector3.Distance (i.rectTransform.position, target) < 0.2f) {
				soundManager.PlayClipAtPoint ("wumpa_collect", transform.position, 0.01f);
				Destroy (i.gameObject);
				yield return new WaitForSeconds(0.1f);
				IncrementWumpaText();
				break;
			} 
			yield return new WaitForSeconds (0);
		}
	}
	
	private void IncrementWumpaText (){
		tempwumpas += 1;
		if (tempwumpas == 100) {
			tempwumpas = 0;
		}
		wumpaText.text = tempwumpas.ToString ();
	}

    public void ShowItem(PickupType type)
    {
        if (ItemImages.ContainsKey(type))
            ItemImages[type].enabled = true;
    }

    public void Reset() {
        foreach (Image i in images) {
            if (i.enabled) i.enabled = false;
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

}
