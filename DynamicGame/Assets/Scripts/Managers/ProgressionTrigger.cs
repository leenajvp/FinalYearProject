using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class ProgressionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player => FindObjectOfType<PlayerController>().gameObject;
    [SerializeField] private DDA.DDAManager ddaManager => FindObjectOfType<DDA.DDAManager>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            ddaManager.SteamThroughDifficulty();
            ddaManager.events += "\n Player have rushed through to second stage, extra npc pool activated " + PlayerPrefs.GetInt("Progression").ToString() + "  " + Time.time.ToString();
        }
    }
}
