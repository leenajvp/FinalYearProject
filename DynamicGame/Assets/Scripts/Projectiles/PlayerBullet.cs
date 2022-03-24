using Bullets;
using DDA;
using Enemies;
using Player;
using UnityEngine;

public class PlayerBullet : BulletController
{
    public int bulletDamange;
    private PlayerController player;

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<PlayerController>();
        Physics.IgnoreLayerCollision(14, 13);
    }

    private void OnCollisionEnter(Collision collision)
    {
        pool.ReturnObject(gameObject);
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.currentHealth -= bulletDamange;
                enemy.gameObject.GetComponent<EnemyBehaviourBase>().isHit = true;

                if (player.isDisguised && !enemy.explorationNPC)
                    player.isDisguised = false;
            }

            
        
    }
}

