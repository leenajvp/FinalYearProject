using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DDA;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public float health = 10;
        public float currentHealth;
       [SerializeField] private DDAManager ddaManager;
       [SerializeField] private SceneMngr sceneMngr;

        private PlayerController player;

        private void Start()
        {
            player = GetComponent<PlayerController>();

            if(ddaManager == null)
            {
                ddaManager = FindObjectOfType<DDAManager>();
            }

            currentHealth = health;
        }

        private void Update()
        {
            if (currentHealth <= 0)
            {
                player.PauseGame();
                ddaManager.playerDead = true;
                Time.timeScale = 0;
                currentHealth = health;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //EnemyBullet eBullet = hit.collider.GetComponent<EnemyBullet>();

            //if (collision.gameObject == gameObject.GetComponent<EnemyBullet>())
            //{
            //    Debug.Log("hit");
            //    health -= 1;
            //}
        }
    }
}
