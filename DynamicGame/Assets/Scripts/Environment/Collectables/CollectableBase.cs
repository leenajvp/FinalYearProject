using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class CollectableBase : MonoBehaviour, ICollectable
{
    [SerializeField] protected Canvas itemUI;
    [SerializeField] protected float detectionDistance = 6f;
    public bool isInventoryItem { get; set; }
    public bool collected { get; set; }

    protected PlayerController playerController;
    protected Transform player;

    protected virtual void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        player = playerController.transform;
    }

    protected virtual void Update()
    {
        Vector3 playerPos = player.transform.position;
        float distance = Vector3.Distance(transform.position, playerPos);

        if (distance < detectionDistance && !collected)
        {
            itemUI.enabled = true;
            itemUI.transform.forward = Camera.main.transform.forward;
        }

        else
        {
            itemUI.enabled = false;
        }
    }
}
