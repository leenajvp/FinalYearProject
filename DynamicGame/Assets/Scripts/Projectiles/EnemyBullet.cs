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
        StartCoroutine(DestroyTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

            if(player != null)
            {
                player.currentHealth -= bulletDamange;
                ddaManager.currentPHits++;
            }

            pool.ReturnObject(gameObject);
        }
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(2);
        pool.ReturnObject(gameObject);
    }
}
