using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("UI for inventory items")]
    [Tooltip("Slot prefab for item in inventory")]
    public Image slotPrefab;
    [Tooltip("Panel to display collected items")]
    [SerializeField] private GameObject inventoryPanel;
    [Header("Starting bullet amount")]
    public int bullets;
    [HideInInspector]
    public List<IQuestItems> collectedQItems = new List<IQuestItems>();
    public event EventHandler<InventoryEventArgs> ItemAdded;

    public void AddItem(IQuestItems item)
    {
        Image slot = Instantiate(slotPrefab);
        slot.transform.SetParent(inventoryPanel.transform, false);
        collectedQItems.Add(item);
        slot.gameObject.GetComponent<Image>().sprite = item.setImage;
    }
}

