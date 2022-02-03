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
            HeadShot headShot = collision.gameObject.GetComponent<HeadShot>();

            if(enemy != null && headShot == null)
            {
                enemy.currentHealth -= bulletDamange;
                ddaManager.currentHits++;
            }

            else if(headShot!=null)
            {
                // need to set headsjot to be under enemy or something as rn the nemy is not being aclled to health cant be reduced
                enemy.currentHealth = 0;
                ddaManager.currentHits++;
            }

            else
            {
                return;
            }
        }
    }
}
