using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bullets;
using DDA;

public class PlayerBullet : BulletController
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
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

            if(enemy != null)
            {
                enemy.currentHealth -= bulletDamange;
                ddaManager.currentHits++;
            }

            else
            {
                return;
            }
        }
    }
}
