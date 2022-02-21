using Player;
using UnityEngine;

public class ElectricLock : MonoBehaviour, Iinteractive
{
    public bool available { get; set; }
    [SerializeField] protected GameObject DoorToOpen;
    [Header("Puzzle UI")]
    [Tooltip("HUD to inform player of available interaction")]
    [SerializeField] protected GameObject puzzleHUDAlert;
    [SerializeField] protected GameObject puzzle;

    protected GameObject player;
    protected PlayerController playerController;
    protected SlidingDoor door;

    protected virtual void Start()
    {
        available = true;
        playerController = FindObjectOfType<PlayerController>();
        door = DoorToOpen.GetComponent<SlidingDoor>();
        player = playerController.gameObject;
        puzzle.SetActive(false);
        puzzleHUDAlert.SetActive(false);
    }

    public void SetUnAvailable()
    {
        playerController.interacting = false;
        available = false;
        puzzle.SetActive(false);
        puzzleHUDAlert.SetActive(false);   
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && available)
        {
            playerController.availableInteraction = true;
            puzzleHUDAlert.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player && isActiveAndEnabled)
        {
            playerController.availableInteraction = false;
            puzzleHUDAlert.SetActive(false);
        }
    }
}
