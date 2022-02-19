using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Tooltip("Set to true if door is unlcoked from start")]
    public bool unlocked = false;
    [SerializeField] private float openDistance = 1f;
    [SerializeField] private float openSpeed = 2f;

    private Vector3 defaultPos;
    private bool open= false;

    private Transform doorPos;

    private void Start()
    {
        defaultPos = transform.position;    
        doorPos = gameObject.transform;
        open = false;
    }

    private void Update()
    {
        if(open && unlocked)
            doorPos.position = new Vector3(doorPos.position.x, doorPos.position.y, Mathf.Lerp(doorPos.position.z, defaultPos.z + (open ? openDistance : 0), Time.deltaTime * openSpeed));

        else
            doorPos.position = new Vector3(doorPos.position.x, doorPos.position.y, Mathf.Lerp(doorPos.position.z, defaultPos.z, Time.deltaTime * openSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!open)
            open = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(open)
            open=false;
    }
}
