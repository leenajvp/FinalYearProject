using Player;
using UnityEngine;

public class CollectableBase : MonoBehaviour, ICollectable
{
    [SerializeField] protected Canvas itemUI;
    [SerializeField] protected float detectionDistance = 6f;
    public bool isInventoryItem { get; set; }
    public bool collected { get; set; }

    protected PlayerController playerController;
    protected Transform player;
    protected float uiYPos;

    protected virtual void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        player = playerController.transform;
        uiYPos = transform.position.y + 0.3f;
    }

    protected virtual void Update()
    {
        Vector3 playerPos = player.transform.position;
        float distance = Vector3.Distance(transform.position, playerPos);

        if (distance < detectionDistance && !collected)
        {
            ActivateUI();
        }

        else if (distance > detectionDistance && distance < detectionDistance + 3)
            itemUI.enabled = false;
    }

    public void Collect()
    {
        itemUI.enabled = false;
        gameObject.SetActive(false);
    }

    protected virtual void ActivateUI()
    {
        itemUI.transform.position = new Vector3(transform.position.x, uiYPos, transform.position.z);
        itemUI.enabled = true;
        itemUI.transform.forward = Camera.main.transform.forward;
    }

    protected virtual void DeactivateUI()
    {
        itemUI.enabled = false;
    }
}
