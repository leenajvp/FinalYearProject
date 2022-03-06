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

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(other != null)
        {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

            if(player != null)
            {
                player.currentHealth -= bulletDamange;
                ddaManager.currentPHits++;
            }
        }
    }
}
