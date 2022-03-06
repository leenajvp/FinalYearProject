using UnityEngine;
using UnityEngine.UI;

public class GeneralCollectable : CollectableBase
{
    public enum CollectableType
    {
        Health,
        Bullets
    }

    [Header("Collectable item type")]
    public CollectableType type;

    [Header("Item UI")]
    [Tooltip("Quantity of the collectable added for player")]
    public int quantity = 5;
    [Tooltip("Text in canvas to display quantity")]
    [SerializeField] protected Text quantityText;

    protected override void Start()
    {
        base.Start();
        isInventoryItem = false;
    }

    protected override void ActivateUI()
    {
        base.ActivateUI();
        quantityText.text = quantity.ToString();
    }
}
