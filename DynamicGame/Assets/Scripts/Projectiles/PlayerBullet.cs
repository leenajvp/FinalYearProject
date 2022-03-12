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
        StartCoroutine(DestroyTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            pool.ReturnObject(gameObject);
            EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.currentHealth -= bulletDamange;
                ddaManager.currentEHits++;
                enemy.gameObject.GetComponent<EnemyBehaviourBase>().isHit = true;

                if (player.isDisguised && !enemy.explorationNPC)
                    player.isDisguised = false;
            }
        }
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(1);
        pool.ReturnObject(gameObject);
    }
}

