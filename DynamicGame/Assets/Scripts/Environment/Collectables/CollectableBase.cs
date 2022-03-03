using Player;
using UnityEngine;
using UnityEngine.UI;

public class CollectableBase : MonoBehaviour, ICollectable
{
    [SerializeField] protected float detectionDistance = 6f;

    [Header("Ite Spatial UI")]
    [Tooltip("The canvas to be activated")]
    [SerializeField] protected Canvas itemUICanvas;

    public bool isInventoryItem { get; set; }
    public bool collected { get; set; }
    protected PlayerController playerController;
    protected Transform player;
    protected float uiYPos;

    protected virtual void Start()
    {
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
