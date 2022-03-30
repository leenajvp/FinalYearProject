using UnityEngine;

public class KeycardDoor : SlidingDoor
{
    [Header("Required item to open")]
    [SerializeField] private GameObject requiredObject1;
    [SerializeField] private GameObject requiredObject2;
    private string requiredObjName1;
    private string requiredObjName2;
    private PlayerInventory pInventory;
    public bool allowNPCEntry= true;

    protected override void Start()
    {
        base.Start();
        requiredObjName1 = requiredObject1.name;
        requiredObjName2 = requiredObject2.name;
        pInventory = player.GetComponent<PlayerInventory>();
        active = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        var collectedObjs = pInventory.collectedQItems;

        if (other.gameObject == base.playerObj)
        {
            if (collectedObjs.Count != 0)
            {
                for (int i = 0; i < collectedObjs.Count; i++)
                {
                    if (collectedObjs[i].name == requiredObjName1 || collectedObjs[i].name == requiredObjName2)
                    {
                        if (!open)
                        {
                            active = true;
                            open = true;
                        }

                    }
                }
            }
        }

        if (allowNPCEntry)
        {
            if (other.gameObject.tag == "NPC" && other.gameObject.GetComponent<EnemyHealth>().explorationNPC==true)
            {
                if (!open)
                {
                    active = true;
                    open = true;
                }
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (open)
        {
            open = false;
            active = false;
        }
    }
}
