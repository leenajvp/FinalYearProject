using UnityEngine;
using System;
using System.Collections;
using Player;
using Enemies;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door slides up on trigger enter")]
    [SerializeField] private float openDistance = 2.5f;
    [SerializeField] private float openSpeed = 2f;
    [Tooltip("Set to true if door is unlcoked from start")]
    public bool active = false;
    [Header("lock color indicating lock status")]
    [SerializeField] private Color unlocked = Color.green;
    [SerializeField] private Color locked = Color.red;
    [Tooltip("Light to direct if door is locked or unlocked")]
    [SerializeField] private GameObject[] lockLights;

    [Header("UI Sprites")]
    [SerializeField] private GameObject currentIcon;
    [SerializeField] private Sprite unlockedIcon;
    [SerializeField] private Sprite lockedIcon;

    private Vector3 defaultPos;
    protected bool open = false;

    private Transform doorPos;
    protected GameObject player;
    protected EnemyBehaviourBase npc;

    protected virtual void Start()
    {
        defaultPos = transform.position;
        doorPos = gameObject.transform;
        open = false;
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    protected virtual void Update()
    {
        MoveDoor();
        UpdateLight();
        UpdateMapSprite();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player || other.gameObject.tag == "NPC")
        {
            if (!open)
                open = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player || other.gameObject.tag == "NPC")
        {
            if (open)
                StartCoroutine(OpenTimer());
        }
    }


    private void MoveDoor()
    {
        if (open && active)
            doorPos.position = new Vector3(doorPos.position.x, Mathf.Lerp(doorPos.position.y, defaultPos.y + openDistance, Time.deltaTime * openSpeed), doorPos.position.z);

        else 
            doorPos.position = new Vector3(doorPos.position.x, Mathf.Lerp(doorPos.position.y, defaultPos.y, Time.deltaTime * openSpeed), doorPos.position.z);
    }

    private void UpdateLight()
    {
        if(active)
            foreach (var light in lockLights)
            light.GetComponent<Renderer>().material.SetColor("_EmissionColor", unlocked);

        else
            foreach (var light in lockLights)
                light.GetComponent<Renderer>().material.SetColor("_EmissionColor", locked);
    }

    private void UpdateMapSprite()
    {
        if (active)
        {
            currentIcon.GetComponent<SpriteRenderer>().sprite = unlockedIcon;
        }

        else
        {
            currentIcon.GetComponent<SpriteRenderer>().sprite = lockedIcon;   
        }
    }

    private IEnumerator OpenTimer()
    {
        yield return new WaitForSeconds(1);
        open = false;
    }

}
