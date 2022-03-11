using UnityEngine;

public class KeycardDoor : SlidingDoor
{
    [Header("Required item to open")]
    [SerializeField] private GameObject requiredObject;
    private string requiredObjName;
    private PlayerInventory pInventory;

    protected override void Start()
    {
        base.Start();
        requiredObjName = requiredObject.name;
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
                    if (collectedObjs[i].name == requiredObjName)
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

        if (other.gameObject.tag == "NPC")
        {
            if (!open)
            {
                active = true;
                open = true;
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
