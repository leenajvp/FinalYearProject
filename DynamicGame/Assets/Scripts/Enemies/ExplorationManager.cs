using UnityEngine;

[RequireComponent(typeof(SlidingDoor))]
public class ExplorationManager : MonoBehaviour
{
    [Header("Door to manage")]
    [SerializeField] private ButtonPuzzleManager buttonPuzzle;
    SlidingDoor door;


    private void Start()
    {
        door = GetComponent<SlidingDoor>();
        door.active = false;
    }

    private void Update()
    {
        if (!buttonPuzzle.notCompleted)
        {
            door.active = true;
        }
    }
}
