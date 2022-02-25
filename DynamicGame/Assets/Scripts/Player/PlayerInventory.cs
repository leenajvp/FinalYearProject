using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInventory : MonoBehaviour
{
    public Image slotPrefab;
    // private readonly List<ICodePiece> collectedObjects = new List<ICodePiece>();
    public int bullets;
    public int codes;
    public List<ICodePiece> codePieces = new List<ICodePiece>();
    public event EventHandler<InventoryEventArgs> ItemAdded;
    [SerializeField]private InventoryHUD hud;

    public void AddItem(ICodePiece item)
    {
        Image slot = Instantiate(slotPrefab);
        slot.transform.SetParent(hud.transform, false);
        codePieces.Add(item);
        codes = codePieces.Count;
        //  item.Collect();

        if (ItemAdded != null)
        {
            ItemAdded(this, new InventoryEventArgs(item));
        }
    }
}

