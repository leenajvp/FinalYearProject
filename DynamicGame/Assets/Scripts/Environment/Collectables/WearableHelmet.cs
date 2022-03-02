using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuestItems))]
public class WearableHelmet : MonoBehaviour
{
    [SerializeField] private GameObject playerHelmet;
    [SerializeField] private GameObject thisHelmet;
    private Color helmetColor;
    private Renderer pRenderer;
    private QuestItems qItem;

    private void Start()
    {
        qItem = GetComponent<QuestItems>();
        helmetColor = thisHelmet.GetComponent<Renderer>().material.color;
        pRenderer = playerHelmet.GetComponent<Renderer>();   
    }

    private void Update()
    {
        if (qItem.inInventory)
        {
            pRenderer.material.color = helmetColor;
        }
    }
}
