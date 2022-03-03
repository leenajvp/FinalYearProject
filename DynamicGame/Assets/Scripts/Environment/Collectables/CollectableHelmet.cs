using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHelmet : QuestItems
{

    [SerializeField] private GameObject playerHelmet;
    [SerializeField] private GameObject thisHelmet;
    private Color helmetColor;
    private Renderer pRenderer;
    private QuestItems qItem;

    protected override void Start()
    {
        base.Start();
        qItem = GetComponent<QuestItems>();
        helmetColor = thisHelmet.GetComponent<Renderer>().material.color;
        pRenderer = playerHelmet.GetComponent<Renderer>();
    }

    public override void MoveToInventory()
    {
        base.MoveToInventory();
        pRenderer.material.color = helmetColor;
    }
}
