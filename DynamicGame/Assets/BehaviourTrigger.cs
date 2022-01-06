using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject setNPC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject==setNPC)
        {
            setNPC.GetComponent<EnemyBehaviour>().sitDown= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == setNPC)
        {
            setNPC.GetComponent<EnemyBehaviour>().sitDown = false;
        }
    }
}
