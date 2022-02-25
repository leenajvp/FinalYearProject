using UnityEngine;
using UnityEngine.UI;

public class CodePieces : CollectableBase, ICodePiece
{
    [Header("Code Piece Moves to camera when interacted with")]
    [SerializeField] private float displayDistance = 1f;
    [SerializeField] private float moveToCamSpeed = 0.1f;
    [Header("Information displayed below item when displayed")]
    [SerializeField] private Text pieceInformation;
    [SerializeField] private string itemInfo = "";

    [Header("Sprite in Inventory")]
    [SerializeField] private Sprite image = null;
    public Sprite setImage
    {
        get
        {
            return image;
        }
    }

    protected override void Start()
    {
        base.Start();
        isInventoryItem = true;
        pieceInformation.text = itemInfo;
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

    private void BringObjectToCam()
    {
        // When player triggers collected, object moves to centre of camera for inspection
        playerController.interacting = true;
        playerController.DisablePlayer();
        itemUI.enabled = false;
        pieceInformation.gameObject.SetActive(true);
        Vector3 displayPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane + displayDistance));
        transform.position = Vector3.Lerp(transform.position, displayPoint, moveToCamSpeed * Time.timeScale);
        transform.LookAt(Camera.main.transform.position);
        playerController.DisablePlayer();
    }

    public void MoveToInventory()
    {
        //WHen player has collected the item and presses E the item will be removed and moved to the inventory
        playerController.interacting = false;
        playerController.DisablePlayer();
        pieceInformation.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
