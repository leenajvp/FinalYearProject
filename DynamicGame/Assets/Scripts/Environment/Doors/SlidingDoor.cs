using Enemies;
using Player;
using System.Collections;
using UnityEngine;
using Player;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door slides up on trigger enter")]
    [SerializeField] private float openDistance = 2.5f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] protected PlayerController player;
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
    protected GameObject playerObj;
    protected EnemyBehaviourBase npc;
   [SerializeField]protected AudioSource openSound;
    [SerializeField]protected AudioSource closeSound;

    protected virtual void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();

        playerObj = player.gameObject;
        defaultPos = transform.position;
        doorPos = gameObject.transform;
        open = false;
    }

    protected virtual void Update()
    {
        MoveDoor();
        UpdateLight();
        UpdateMapSprite();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObj || other.gameObject.tag == "NPC")
        {
            if (!open && active)
            {
                open = true;
                openSound.Play();
            }
                
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerObj || other.gameObject.tag == "NPC")
        {
            if (open)
            {
                StartCoroutine(OpenTimer());
                
            }
        }
    }

    private void MoveDoor()
    {
        if (open && active)
            doorPos.position = new Vector3(doorPos.position.x, Mathf.Lerp(doorPos.position.y, defaultPos.y + openDistance, Time.deltaTime * openSpeed), doorPos.position.z); //open up

        else
            doorPos.position = new Vector3(doorPos.position.x, Mathf.Lerp(doorPos.position.y, defaultPos.y, Time.deltaTime * openSpeed), doorPos.position.z); //back to default
    }

    private void UpdateLight()
    {
        if (active)
            foreach (var light in lockLights)
                light.GetComponent<Renderer>().material.SetColor("_EmissionColor", unlocked);

        else
            foreach (var light in lockLights)
                light.GetComponent<Renderer>().material.SetColor("_EmissionColor", locked);
    }

    private void UpdateMapSprite() //Mini-map
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
        closeSound.Play();
    }

}
