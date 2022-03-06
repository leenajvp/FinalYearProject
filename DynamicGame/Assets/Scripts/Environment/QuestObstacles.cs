using Player;
using UnityEngine;
using UnityEngine.UI;

public class QuestObstacles : MonoBehaviour, Iinteractive
{
    public bool available { get; set; }
    [Header("Object required to pass")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject requiredObject;
    [Header("Item UI")]
    [Tooltip("UI to inform player of available interaction")]
    [SerializeField] private GameObject itemInfo;
    [SerializeField] private Text displayText;
    [SerializeField] private Text actionText;
    [Tooltip("Text to inform player can proceed")]
    [TextArea(2, 2)]
    [SerializeField] private string succeedText = "";
    [Tooltip("Text to inform player cannot proceed")]
    [TextArea(2, 2)]
    [SerializeField] private string failedText = "";
    [Tooltip("Text to inform how to proceed")]
    [TextArea(2, 2)]
    [SerializeField] private string succeedAction = "";
    [Tooltip("Text to inform how to proceed")]
    [TextArea(2, 2)]
    [SerializeField] private string failedAction = "";
    private string requiredObjName;
    private bool canOpen = false;
    private PlayerInventory pInventory;

    protected virtual void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();

        requiredObjName = requiredObject.name;
        available = true;
        pInventory = player.GetComponent<PlayerInventory>();
        itemInfo.SetActive(false);
    }

    public void SetUnAvailable()
    {
        player.interacting = false;
        player.DisablePlayer();
        itemInfo.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (canOpen && player.interactAction.triggered) // interaction triggered with same key after correct item checked
        {
            itemInfo.SetActive(false);
            player.interacting = false;
            player.DisablePlayer();
            gameObject.SetActive(false);
        }
    }

    public void GetInteraction()
    {
        if (!itemInfo.activeSelf) // If interaction triggered while canvas is not active, pause game and activate item UI
        {
            player.interacting = true;
            player.DisablePlayer();

            var collectedObjs = pInventory.codePieces;

            if (collectedObjs.Count != 0)
            {
                for (int i = 0; i < collectedObjs.Count; i++)
                {
                    if (collectedObjs[i].name == requiredObjName)
                    {
                        actionText.text = succeedAction;
                        displayText.text = succeedText;
                        itemInfo.SetActive(true);
                        canOpen = true;
                        return;
                    }

                    else
                        continue;
                }
            }

            actionText.text = failedAction;
            displayText.text = failedText;
            itemInfo.SetActive(true);
            return;
        }

        // if item UI is currently active, return to game
        itemInfo.SetActive(false);
        player.interacting = false;
        player.DisablePlayer();
    }
}
