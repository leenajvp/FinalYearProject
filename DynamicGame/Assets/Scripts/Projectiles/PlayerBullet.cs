using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using Bullets;
using DDA;
using Player;

public class PlayerBullet : BulletController
{
    public int bulletDamange;
    private DDAManager ddaManager;
    private PlayerController player;

    protected override void Start()
    {
        base.Start();

        ddaManager = FindObjectOfType<DDAManager>();
        player = FindObjectOfType<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.currentHealth -= bulletDamange;
                ddaManager.currentEHits++;
                enemy.gameObject.GetComponent<EnemyBehaviourBase>().isHit = true;

                if (player.isDisguised && !enemy.explorationNPC)
                    player.isDisguised = false;
            }

            pool.ReturnObject(gameObject);
        }

        
    }
}

