using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class ExplorationCheckPoints : MonoBehaviour
{
    [SerializeField] private GameObject player => FindObjectOfType<PlayerController>().gameObject;
    [SerializeField] private SceneMngr sceneMngr => FindObjectOfType<SceneMngr>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            sceneMngr.startpos = gameObject.transform;
        }
    }
}
