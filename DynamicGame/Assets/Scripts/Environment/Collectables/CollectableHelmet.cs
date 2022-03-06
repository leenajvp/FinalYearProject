using UnityEngine;

public class CollectableHelmet : QuestItems
{
    [Tooltip("Player's helmet mesh that will change material")]
    [SerializeField] private GameObject playerHelmet;
    public Color helmetColor;
    private Renderer pRenderer;
    private QuestItems qItem;

    protected override void Start()
    {
        base.Start();
        qItem = GetComponent<QuestItems>();
        helmetColor = GetComponent<Renderer>().material.color;
        pRenderer = playerHelmet.GetComponent<Renderer>();
    }

    public override void MoveToInventory()
    {
        base.MoveToInventory();
        pRenderer.material.color = helmetColor;
    }
}
