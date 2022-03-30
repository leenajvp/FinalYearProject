using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player.PlayerHealth))]
public class PlayerInventory : MonoBehaviour
{
    [Header("UI for inventory items")]
    [Tooltip("Slot prefab for item in inventory")]
    public Image slotPrefab;
    [Tooltip("Panel to display collected items")]
    [SerializeField] private GameObject inventoryPanel;
    [Header("Starting bullet amount")]
    public int bullets;
    public List<IQuestItems> collectedQItems = new List<IQuestItems>();
    public event EventHandler<InventoryEventArgs> ItemAdded;

    [HideInInspector] public bool bossCardReceived = false;

    private Player.PlayerHealth pHealth;

    private void Start()
    {
        bossCardReceived = false;
    }

    public void AddItem(IQuestItems item)
    {
        Image slot = Instantiate(slotPrefab);
        slot.transform.SetParent(inventoryPanel.transform, false);
        collectedQItems.Add(item);
        slot.gameObject.GetComponent<Image>().sprite = item.setImage;
        // Debug.Log(item.name + " " + )
    }

    public void RemoveItem()
    {
        if (collectedQItems.Count != 0)
        {
            for (int i = 0; i < collectedQItems.Count; i++)
            {
                if (collectedQItems[i].name == "BKeycard")
                {
                    for (int j = 0; j < inventoryPanel.transform.childCount; j++)
                    {
                        if (inventoryPanel.transform.GetChild(j).GetComponent<Image>().sprite.name == collectedQItems[j].setImage.name)
                        {
                            Destroy(inventoryPanel.transform.GetChild(j).gameObject);
                        }
                    }
                    collectedQItems.Remove(collectedQItems[i]);
                }
            }
        }
    }
}


