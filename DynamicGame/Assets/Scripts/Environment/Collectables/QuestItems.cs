using UnityEngine;
using UnityEngine.UI;

public class QuestItems : CollectableBase, IQuestItems
{
    [SerializeField] private string ObjectName = "quest item";
    [Header("Code Piece Moves to camera when interacted with")]
    [SerializeField] private float displayDistance = 1f;
    [SerializeField] private float moveToCamSpeed = 0.1f;
    [Header("Information displayed below item when displayed")]
    [Header("UI information")]
    [SerializeField] GameObject informationPanel;
    [SerializeField] private Text pieceInformation;
    [SerializeField] private Text actionText;
    [TextArea(2,2)]
    [SerializeField] private string itemInfo = "";
    [TextArea(2, 2)]
    [SerializeField] private string actionInfo = "";
    [Header("Sprite in Inventory")]
    [SerializeField] private Sprite image = null;
    [HideInInspector] public bool inInventory = false;
    public Sprite setImage { get { return image; } }
    public string name { get { return ObjectName; } }

    protected override void Start()
    {
        gameObject.name = name;
        base.Start();
        isInventoryItem = true;
    }

    protected override void Update()
    {
        base.Update();
        CheckStatus();
    }

    private void CheckStatus()
    {
        if (collected)
        {
            BringObjectToCam();

            if (playerController.interactAction.triggered)
            {
                MoveToInventory();
            }
        }
    }

    protected virtual void BringObjectToCam()
    {
        // When player triggers collected, object moves to centre of camera for inspection
        pieceInformation.text = itemInfo;
        playerController.interacting = true;
        playerController.DisablePlayer();
        itemUICanvas.enabled = false;
        informationPanel.SetActive(true);
        Vector3 displayPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane + displayDistance));
        transform.position = Vector3.Lerp(transform.position, displayPoint, moveToCamSpeed * Time.timeScale);
        transform.LookAt(Camera.main.transform.position);
        playerController.DisablePlayer();
    }

    public virtual void MoveToInventory()
    {
        //WHen player has collected the item and presses E the item will be removed and moved to the inventory
        inInventory = true;
        playerController.interacting = false;
        playerController.DisablePlayer();
        informationPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}
