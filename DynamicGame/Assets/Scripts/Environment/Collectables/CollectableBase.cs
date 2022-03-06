using Player;
using UnityEngine;

public class CollectableBase : MonoBehaviour, ICollectable
{
    [Header("Item's Spatial UI")]
    [SerializeField] protected Canvas itemUICanvas;
    [Header("Distance to player for UI activation")]
    [SerializeField] protected float detectionDistance = 6f;
    [SerializeField] protected PlayerController playerController;
    public bool isInventoryItem { get; set; }
    public bool collected { get; set; }
    
    protected Transform player;
    protected float uiYPos;

    protected virtual void Start()
    {
        if (player == null)
            playerController = FindObjectOfType<PlayerController>();

        player = playerController.transform;
        collected = false;
    }

    protected virtual void Update()
    {
        Vector3 playerPos = player.transform.position;
        float distance = Vector3.Distance(transform.position, playerPos);

        if (distance < detectionDistance && !collected)
            ActivateUI();

        else if (distance > detectionDistance && distance < detectionDistance + 3)
            itemUICanvas.enabled = false;
    }

    public virtual void Collect()
    {
        itemUICanvas.enabled = false;
        gameObject.SetActive(false);
    }

    protected virtual void ActivateUI()
    {
        uiYPos = transform.position.y + 0.4f;
        itemUICanvas.transform.position = new Vector3(transform.position.x, uiYPos, transform.position.z);
        itemUICanvas.enabled = true;
        itemUICanvas.transform.forward = Camera.main.transform.forward;
    }

    protected virtual void DeactivateUI()
    {
        itemUICanvas.enabled = false;
    }
}
