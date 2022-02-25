using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bullets;
using DDA;
using Player;

public class EnemyBullet : BulletController
{
    public int bulletDamange;
    [SerializeField] private DDAManager ddaManager;


    protected override void Start()
    {
        base.Start();

        ddaManager = FindObjectOfType<DDAManager>();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if(collision != null)
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

            if(player != null)
            {
                player.currentHealth -= bulletDamange;
                ddaManager.currentPHits++;
            }
        }
    }
}
