using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth), typeof (Enemies.EnemyBehaviourBase))]
public class BossSpawnCheck : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private Enemies.EnemyBehaviourBase eBehaviour;
    private PlayerInventory playerInventory => FindObjectOfType<PlayerInventory>();

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        eBehaviour = GetComponent<Enemies.EnemyBehaviourBase>();
    }

    private void FixedUpdate()
    {
        if(enemyHealth.currentHealth <= 0 && enemyHealth.objectToSpawn)
        {
            playerInventory.bossCardReceived = true;
        }
    }
}
