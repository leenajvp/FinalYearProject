using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : CollectableBase
{
    public int numberOfHealth = 5;

    protected override void Start()
    {
        base.Start();
        isInventoryItem = false;
    }
}
