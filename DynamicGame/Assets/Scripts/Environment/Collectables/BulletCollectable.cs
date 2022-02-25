using UnityEngine;

public class BulletCollectable : CollectableBase
{
    public int numberOfBullets = 5;

    protected override void Start()
    {
        base.Start();
        isInventoryItem = false;
    }
}
