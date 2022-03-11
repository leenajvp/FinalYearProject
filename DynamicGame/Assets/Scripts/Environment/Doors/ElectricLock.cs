using Player;
using UnityEngine;

public class ElectricLock : MonoBehaviour, Iinteractive
{
    public bool notCompleted { get; set; }
    [Header("Connected door settings")]
    [Tooltip("Set to True if lock is available even after completion")]
    [SerializeField] private bool keepAvailable;
    [SerializeField] protected GameObject DoorToManage;

    [Header("Puzzle UI")]
    [Tooltip("HUD to inform player of available interaction")]
    [SerializeField] protected GameObject puzzle;

    protected GameObject player;
    protected Player.PlayerController playerController;
    protected SlidingDoor door;

    protected virtual void Start()
    {
        notCompleted = true;
        playerController = FindObjectOfType<PlayerController>();
        player = playerController.gameObject;
        door = DoorToManage.GetComponent<SlidingDoor>();
        puzzle.SetActive(false);
    }

    public void SetUnAvailable()
    {
        playerController.interacting = false;
        playerController.DisablePlayer();
        puzzle.SetActive(false);

        if (!keepAvailable)
            notCompleted = false;
    }

    public void GetInteraction()
    {
        if (!puzzle.activeSelf && notCompleted)
        {
            puzzle.SetActive(true);
            playerController.interacting = true;
            playerController.DisablePlayer();
        }

        else
        {
            puzzle.SetActive(false);
            playerController.interacting = false;
            playerController.DisablePlayer();
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
}
