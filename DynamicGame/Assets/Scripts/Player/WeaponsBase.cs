using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsBase : MonoBehaviour
{
    public bool inUse;
    public int idNum;
    GameObject player;
    PlayerMovement playerScript;
    MeshRenderer objectRenderer;

    public virtual void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        playerScript = player.GetComponent<PlayerMovement>();
        objectRenderer = GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(playerScript.currentGun == idNum)
        {
            inUse = true;
        }

        else
        {
            inUse=false;
        }

        SetActive();
    }

    public void SetActive()
    {
        if (inUse)
            objectRenderer.enabled=true;
        

        else
            objectRenderer.enabled = false;
    }
}
