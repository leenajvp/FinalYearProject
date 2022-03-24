using UnityEngine;
using Player;

public class CollectableHelmet : QuestItems
{
    [Tooltip("Player's helmet mesh that will change material")]
    [SerializeField] private GameObject playerHelmet;
    [Header("Enemies to set active")]
    [SerializeField] private GameObject npc;
    public Color helmetColor;
    private Renderer pRenderer;
    private QuestItems qItem;

    protected override void Start()
    {
        base.Start();
        qItem = GetComponent<QuestItems>();
        helmetColor = GetComponent<Renderer>().material.color;
        pRenderer = playerHelmet.GetComponent<Renderer>();
        npc.SetActive(false);
    }

    public override void MoveToInventory()
    {
        base.MoveToInventory();
        pRenderer.material.color = helmetColor;
        player.gameObject.GetComponent<PlayerController>().isDisguised = true; // fix this line
        npc.SetActive(true);
    }
}
