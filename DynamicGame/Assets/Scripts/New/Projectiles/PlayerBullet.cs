using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bullets;
using DDA;

public class PlayerBullet : BulletController
{
    public int bulletDamange;
    [SerializeField] private DDAManager ddaManager;

    private void Start()
    {
        ddaManager = FindObjectOfType<DDAManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {

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
                enemy.currentHealth -= bulletDamange*2;
                ddaManager.currentHits++;
            }

            else
            {
                return;
            }
        }
    }
}
