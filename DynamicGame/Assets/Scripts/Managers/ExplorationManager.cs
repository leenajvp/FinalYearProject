using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
    [Header("Activate door for second part")]
    [SerializeField] private ButtonPuzzleManager buttonPuzzle;
    [SerializeField] private SlidingDoor extraDoorToOpen;

    [Header("Choose Keycard npc")]
    [SerializeField] CollectableHelmet helmet;
    [SerializeField] private GameObject npcToActivate;
    [SerializeField] private GameObject boss;

    private void Start()
    {
        extraDoorToOpen.active = false;
        boss.GetComponent<EnemyHealth>().spawnObject = true;
    }

    private void Update()
    {
        if (!buttonPuzzle.notCompleted)
        {
            extraDoorToOpen.active = true;
        }

        // If player collects the helmet to disguise, activate npc to enter with keycard and remove keycard from the boss
        if (helmet.inInventory)
        {
            npcToActivate.SetActive(true);
            boss.GetComponent<EnemyHealth>().spawnObject = false;
            helmet.inInventory = false;
        }
    }
}
