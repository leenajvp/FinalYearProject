using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class ExplorationCheckPoints : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private SceneMngr sceneMngr;

    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>().gameObject;

        if(sceneMngr == null)
            sceneMngr= FindObjectOfType<SceneMngr>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            sceneMngr.startpos = gameObject.transform;
        }
    }
}
