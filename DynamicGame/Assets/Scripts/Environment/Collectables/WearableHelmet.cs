using UnityEngine;

[RequireComponent(typeof(QuestItems))]
public class WearableHelmet : MonoBehaviour
{
    [Tooltip("Player's helmet mesh that will change material")]
    [SerializeField] private GameObject playerHelmet;
    private Color helmetColor;
    private Renderer pRenderer;
    private QuestItems qItem;

    private void Start()
    {
        qItem = GetComponent<QuestItems>();
        helmetColor = GetComponent<Renderer>().material.color;
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
