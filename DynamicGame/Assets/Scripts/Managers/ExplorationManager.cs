using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
    [Header("Activate door for second part")]
    [SerializeField] private ButtonPuzzleManager buttonPuzzle;
    [SerializeField] private SlidingDoor extraDoorToOpen;
    [SerializeField] private GameObject addNPC;
    private bool activated = false;

    [Header("Choose Keycard npc")]
    [SerializeField] CollectableHelmet helmet;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject explorationNpc;
    private EnemyHealth bHealth;

    [Header("Manage GamePath")]
    [SerializeField] private ChangeLocks locks;

    

    private void Start()
    {
        extraDoorToOpen.active = false;
        boss.GetComponent<EnemyHealth>().spawnObject = true;
        addNPC.SetActive(false);
        bHealth= boss.GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (!buttonPuzzle.notCompleted && !activated)
        {
            extraDoorToOpen.active = true;
            addNPC.SetActive(true);
            activated = true;
            locks.activateExploration = true;
            
        }

        // If player collects the helmet to disguise, activate npc to enter with keycard and remove keycard from the boss
        if (helmet.inInventory)
        {
            boss.GetComponent<EnemyHealth>().spawnObject = false;
            helmet.inInventory = false;
        }

        if(bHealth.currentHealth <= 0)
        {
            explorationNpc.GetComponent<EnemyHealth>().spawnObject= false;
        }
    }
}
