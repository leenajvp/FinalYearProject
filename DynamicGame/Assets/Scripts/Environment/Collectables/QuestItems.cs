using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestItems : CollectableBase, IQuestItems
{
    [SerializeField] private string ObjectName = "quest item";
    [Header("Code Piece Moves to camera when interacted with")]
    [SerializeField] private float displayDistance = 1.0f;
    [SerializeField] private float moveToCamSpeed = 0.1f;
    [Header("Information displayed below item when displayed")]
    [SerializeField] GameObject informationPanel;
    [Tooltip("Description of collected item")]
    [SerializeField] private Text pieceInformation;
    [Tooltip("Advises player how to proceed")]
    [SerializeField] private Text actionText;
    [TextArea(2, 2)]
    [SerializeField] private string itemInfo = "";
    [TextArea(2, 2)]
    [SerializeField] private string actionInfo = "";
    [Header("Sprite in Inventory")]
    [SerializeField] private Sprite image = null;
    [HideInInspector] public bool inInventory = false;
    protected float shootTimer = 0.0f;
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
            StartCoroutine(status());   //Unity input actions trigger multiple times... IEnumerator as workaround for now
        }
    }

    private IEnumerator status()
    {
        yield return new WaitForSeconds(1);
        if (playerController.interactAction.triggered)
        {
            MoveToInventory();
        }
    }

    // When player triggers collected, object moves to centre of camera for inspection
    protected virtual void BringObjectToCam()
    {
        playerController.interacting = true;
        //set UI
        itemUICanvas.enabled = false;
        pieceInformation.text = itemInfo;
        informationPanel.SetActive(true);
        // Move object to camera and rotate to look towards
        Vector3 displayPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane + displayDistance));
        transform.position = Vector3.Lerp(transform.position, displayPoint, moveToCamSpeed * Time.timeScale);
        transform.LookAt(Camera.main.transform.position);
        playerController.DisablePlayer();
    }

    public virtual void MoveToInventory()
    {
        //When player has collected the item and presses E the item will be removed and moved to the inventory
        inInventory = true;
        playerController.interacting = false;
        playerController.DisablePlayer();
        informationPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}
