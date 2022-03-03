using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    public void Collect();
    public bool isInventoryItem { get; set; }
    public bool collected { get; set; }
}
