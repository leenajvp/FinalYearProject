using Player;
using UnityEngine;

public class ElectricLock : MonoBehaviour, Iinteractive
{
    public bool available { get; set; }
    [Tooltip("Set to True if lock is available even after completion")]
    [SerializeField] private bool keepAvailable;
    [SerializeField] protected GameObject DoorToManage;
    [Header("Puzzle UI")]
    [Tooltip("HUD to inform player of available interaction")]
    [SerializeField] protected GameObject puzzle;

    protected GameObject player;
    protected PlayerController playerController;
    protected SlidingDoor door;

    protected virtual void Start()
    {
        available = true;
        playerController = FindObjectOfType<PlayerController>();
        player = playerController.gameObject;
        door = DoorToManage.GetComponent<SlidingDoor>();
        puzzle.SetActive(false);
    }

    public void SetUnAvailable()
    {
        playerController.interacting = false;
        puzzle.SetActive(false);

        if(!keepAvailable)
            available = false;
    }

    public void GetPuzzle()
    {
        if (!puzzle.activeSelf && available)
        {
            puzzle.SetActive(true);
            playerController.interacting = true;
        }

        else
        {
            puzzle.SetActive(false);
            playerController.interacting = false;
        }
    }
    public void Unlock()
    {
        door.active = true;
    }

    public void Lock()
    {
        door.active = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject == player && available)
    //    {
    //       // playerController.availableInteraction = true;
    //        puzzleHUDAlert.SetActive(true);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject == player && isActiveAndEnabled)
    //    {
    //       // playerController.availableInteraction = false;
    //        puzzleHUDAlert.SetActive(false);
    //    }
    //}
}
