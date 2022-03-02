using Player;
using UnityEngine;
using Enemies;

public class KeycardDoor : SlidingDoor
{
    [SerializeField] private GameObject requiredObject;
    private string requiredObjName;
    private PlayerController pController;
    private PlayerInventory pInventory;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        requiredObjName = requiredObject.name;
        pController = FindObjectOfType<PlayerController>();
        pInventory = pController.GetComponent<PlayerInventory>();
        active = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        var collectedObjs = pInventory.codePieces;

        if (other.gameObject == player)
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


        if ( other.gameObject.tag == "NPC")
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
