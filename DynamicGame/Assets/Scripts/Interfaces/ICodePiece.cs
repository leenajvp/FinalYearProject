using System;
using UnityEngine;

public interface ICodePiece 
{
    Sprite setImage { get; }
}

public class InventoryEventArgs : EventArgs
{
    public ICodePiece Item;

    public InventoryEventArgs(ICodePiece collectedItem)
    {
        Item = collectedItem;
    }
}
