using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bullets;
using DDA;
using Player;

public class EnemyBullet : BulletController
{
    public int bulletDamange;

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

            if(player != null)
            {
                player.currentHealth -= bulletDamange;
            }

            pool.ReturnObject(gameObject);
        }
    }
}
