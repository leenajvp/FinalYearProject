using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuestObstacles : MonoBehaviour, Iinteractive
{
    public bool available { get; set; }
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
    [SerializeField] private GameObject requiredObject;
    private string requiredObjName;
    private bool canOpen = false;
    private PlayerController pController;
    private PlayerInventory pInventory;

    private PlayerInput playerInput;

    protected virtual void Start()
    {
        requiredObjName = requiredObject.name;
        available = true;
        pController = FindObjectOfType<PlayerController>();
        pInventory = pController.GetComponent<PlayerInventory>();
        itemInfo.SetActive(false);
    }

    public void SetUnAvailable()
    {
        pController.interacting = false;
        pController.DisablePlayer();
        itemInfo.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (canOpen && pController.interactAction.triggered)
        {
            itemInfo.SetActive(false);
            pController.interacting = false;
            pController.DisablePlayer();
            gameObject.SetActive(false);
        }
    }

    public void GetInteraction()
    {
        if (!itemInfo.activeSelf)
        {
            pController.interacting = true;
            pController.DisablePlayer();

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
                    }

                    else
                    {
                        actionText.text = failedAction;
                        displayText.text = failedText;
                        itemInfo.SetActive(true);
                    }
                }
            }

            else
            {
                actionText.text = failedAction;
                displayText.text = failedText;
                itemInfo.SetActive(true);
            }
        }

        else
        {
            itemInfo.SetActive(false);
            pController.interacting = false;
            pController.DisablePlayer();
        }
    }
}
