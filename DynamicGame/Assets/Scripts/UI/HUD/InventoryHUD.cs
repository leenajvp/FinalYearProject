using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

public class InventoryHUD : MonoBehaviour
{
    private PlayerInventory inventory;
   [SerializeField] private Transform inventoryPanel;

    private void Awake()
    {
        inventory = FindObjectOfType<PlayerInventory>();


        inventory.ItemAdded += Inventory_ItemBeenAdded;
    }

    private void Inventory_ItemBeenAdded(object sender, InventoryEventArgs e)
    {
        foreach (Transform slot in inventoryPanel)
        {
            Transform imageTransform = slot.GetChild(0);
            Image image = imageTransform.GetComponent<Image>();

            if (!image.enabled)
            {
                image.enabled = true;
                image.sprite = e.Item.setImage;

                break;
            }
        }
    }
}
