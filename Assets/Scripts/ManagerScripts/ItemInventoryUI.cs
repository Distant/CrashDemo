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

    // Use this for initialization
    void Start()
    {

        ItemImages = new Dictionary<PickupType, Image>();
        for (int i = 0; i < Mathf.Min(itemTypes.Length, images.Length); i++)
        {
            ItemImages.Add(itemTypes[i], images[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

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

}
