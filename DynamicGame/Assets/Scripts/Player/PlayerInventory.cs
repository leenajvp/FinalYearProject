using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public Image slotPrefab;
    public int bullets;
    public int codes;
    public List<IQuestItems> codePieces = new List<IQuestItems>();
    public event EventHandler<InventoryEventArgs> ItemAdded;
    [SerializeField] private GameObject inventoryPanel;

    public void AddItem(IQuestItems item)
    {
        Image slot = Instantiate(slotPrefab);
        slot.transform.SetParent(inventoryPanel.transform, false);
        codePieces.Add(item);
        codes = codePieces.Count;
        slot.gameObject.GetComponent<Image>().sprite = item.setImage;
    }
}

