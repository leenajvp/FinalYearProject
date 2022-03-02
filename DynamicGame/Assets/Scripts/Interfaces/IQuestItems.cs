using System;
using UnityEngine;

public interface IQuestItems 
{
    Sprite setImage { get; }
    public string name { get; }
}

public class InventoryEventArgs : EventArgs
{
    public IQuestItems Item;

    public InventoryEventArgs(IQuestItems collectedItem)
    {
        Item = collectedItem;
    }
}
