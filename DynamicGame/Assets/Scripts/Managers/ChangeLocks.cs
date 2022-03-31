using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class ChangeLocks : MonoBehaviour
{
    public  bool activateExploration = false;
    public  bool activateSurvivor = true;
    [SerializeField] private GameObject door;
    [SerializeField] private ElectricLock doorLock;
    [SerializeField] private GameObject noEntryUI;
    [SerializeField] private GameObject openLockUI;
    private GameObject player => FindObjectOfType<PlayerController>().gameObject;
    private DDA.DDAManager ddaManager => FindObjectOfType<DDA.DDAManager>();

    public  bool entered;

    private void OnTriggerEnter(Collider other)
    {
        if (!entered && other.gameObject == player)
        {
            if (activateExploration)
            {
                doorLock.puzzle = noEntryUI;
                door.GetComponent<KeycardDoor>().enabled = true;
                ddaManager.events += "\n Player have entered half way on exploration path, survivor path disabled " + Time.time.ToString();
            }

            else
            {
                doorLock.puzzle = openLockUI;
                door.GetComponent<SlidingDoor>().enabled = true;
                ddaManager.events += "\n Player have entered half way on survivor path, exploration path disabled " + Time.time.ToString();
            }
            entered = true;
        }
    }
}
